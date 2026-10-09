using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Reception.Services;
using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;
using TrayGarden.Services.PlantServices.IsEnabledObserver;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.Services.PlantServices.UserNotifications.Core;

using Application = System.Windows.Application;
using MessageBoxImage = System.Windows.MessageBoxImage;

namespace UngoogledChromiumUpdaterPlant;

/// <summary>
/// Checks for a new release and downloads it ahead of time. A downloaded update is installed right away while Chromium is
/// not running; otherwise a toast offers it and the tray icon installs it on click.
/// </summary>
public sealed class UpdateController : IAdvancedStandaloneIcon,
                                       INotifyIconVisibilityControl,
                                       IIsEnabledObserver,
                                       IExtendsGlobalMenu,
                                       IProvidesRareCommands,
                                       IGetPowerOfUserNotifications
{
  private const string DialogCaption = "Ungoogled Chromium updater";
  private const string InstallNowId = "install";

  private static readonly TimeSpan RetryDelay = TimeSpan.FromMinutes(30);
  private static readonly TimeSpan ExitTimeout = TimeSpan.FromSeconds(30);

  private readonly SemaphoreSlim _checkGate = new(1, 1);
  private NotifyIcon _notifyIcon;
  private IPlantEnabledInfo _enabledInfo;
  private IUserNotifier _notifier;
  private string _offeredTag;
  private CancellationTokenSource _loopCts;
  private StagedUpdate _staged;
  private IconState _iconState;
  private bool _installing;

  public static UpdateController Instance { get; } = new();

  public event EventHandler IsIconVisibleChanged;

  public bool IsIconVisible => _iconState != IconState.Hidden;

  public NotifyIcon GetNotifyIcon()
  {
    _notifyIcon = new NotifyIcon
    {
      Text = "Ungoogled Chromium update",
      Icon = UpdateIconFactory.Create(InstalledChromium.TryDetect()?.ExePath ?? string.Empty, UpdateIconKind.UpdateReady),
      Visible = false,
      ContextMenuStrip = CreateContextMenu()
    };
    _notifyIcon.MouseClick += OnIconMouseClick;
    return _notifyIcon;
  }

  public void ConsumeIsEnabledInfo(IPlantEnabledInfo plantEnabledInfo)
  {
    _enabledInfo = plantEnabledInfo;
    _enabledInfo.IsEnabledChanged += (_, _) => ApplyEnabledState();
  }

  public void StoreNotifier(IUserNotifier notifier)
  {
    _notifier = notifier;
  }

  public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
  {
    menuAppender.AppentMenuStripItem(
      "Check for Ungoogled Chromium update",
      null,
      (_, _) => _ = CheckNowAsync());
    return true;
  }

  public List<IRareCommand> GetRareCommands()
  {
    return
    [
      new SimpleRareCommand(
        "Check now",
        "Looks for a new Ungoogled Chromium release and tells right away whether one is available. A new release is then downloaded in the background and the tray icon appears to track it and to install it.",
        () => _ = CheckNowAsync())
    ];
  }

  public void Start()
  {
    Application.Current.Exit += (_, _) => StopLoop();
    AdoptPendingInstaller();
    ApplyEnabledState();
  }

  /// <summary>
  /// An installer left by an earlier run was verified before it got its name, so it is shown as ready right away. The
  /// first check then confirms it is still the latest release and replaces or removes it otherwise.
  /// </summary>
  private void AdoptPendingInstaller()
  {
    PendingInstaller pending = UpdateStaging.FindPending();
    InstalledChromium installed = InstalledChromium.TryDetect();
    if (pending == null || installed == null || pending.ChromiumVersion <= installed.Version)
    {
      UpdateStaging.Sweep(null);
      return;
    }

    _staged = new StagedUpdate(pending.Tag, pending.ChromiumVersion, pending.Path);
    SetIconState(IconState.Ready, installed.ExePath, $"Ungoogled Chromium {pending.Tag} is ready - click to install");
  }

  private void ApplyEnabledState()
  {
    if (_enabledInfo == null || _enabledInfo.IsEnabled)
    {
      StartLoop();
    }
    else
    {
      StopLoop();
    }
  }

  private void StartLoop()
  {
    if (_loopCts != null)
    {
      return;
    }

    _loopCts = new CancellationTokenSource();
    CancellationToken token = _loopCts.Token;
    _ = Task.Run(() => RunLoopAsync(token), token);
  }

  private void StopLoop()
  {
    _loopCts?.Cancel();
    _loopCts = null;
  }

  private async Task RunLoopAsync(CancellationToken cancellationToken)
  {
    while (!cancellationToken.IsCancellationRequested)
    {
      TimeSpan delay;
      try
      {
        await CheckOnceAsync(cancellationToken);
        await HandleReadyUpdateAsync();
        delay = PlantConfiguration.Instance.ResolveCheckInterval();
      }
      catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
      {
        return;
      }
      catch (Exception exception)
      {
        Log.For(this).Warning(exception, "Ungoogled Chromium update check failed");
        delay = RetryDelay;
      }

      try
      {
        await Task.Delay(delay, cancellationToken);
      }
      catch (OperationCanceledException)
      {
        return;
      }
    }
  }

  /// <summary>
  /// Answers as soon as the latest release is known, without waiting for the download, which continues in the background
  /// and is tracked by the tray icon.
  /// </summary>
  private async Task CheckNowAsync()
  {
    if (_checkGate.CurrentCount == 0 || _installing)
    {
      Report(DescribeBusyState(), false);
      return;
    }

    try
    {
      await CheckOnceAsync(CancellationToken.None, check => _ = Task.Run(() => Report(check.Describe(), check.Installed == null)));
      await HandleReadyUpdateAsync();
    }
    catch (Exception exception)
    {
      Log.For(this).Warning(exception, "Manual Ungoogled Chromium update check failed");
      Report($"Update check failed: {exception.Message}", true);
    }
  }

  private string DescribeBusyState()
  {
    return _iconState switch
    {
      IconState.Installing => "Ungoogled Chromium is being updated right now.",
      IconState.Ready => $"Ungoogled Chromium {_staged?.Tag} is downloaded. Click the tray icon to install it.",
      _ => "An update check or download is already in progress. Watch the tray icon."
    };
  }

  private async Task InstallAndReportAsync(WhenRunning whenRunning)
  {
    string error = await InstallAsync(whenRunning);
    if (error != null)
    {
      Report(error, true);
    }
  }

  /// <summary>
  /// Installs a downloaded update without asking while Chromium is not running, so nothing is ever closed behind the user's
  /// back. While it is running, offers the install once per release through a toast; the tray icon remains as the fallback.
  /// </summary>
  private async Task HandleReadyUpdateAsync()
  {
    InstalledChromium installed = InstalledChromium.TryDetect();
    if (_staged == null || _installing || _iconState != IconState.Ready || installed == null)
    {
      return;
    }

    string tag = _staged.Tag;
    if (ChromiumCloser.IsRunning(installed))
    {
      if (_offeredTag != tag)
      {
        _offeredTag = tag;
        _ = OfferInstallAsync(tag);
      }

      return;
    }

    string error = await InstallAsync(WhenRunning.Skip);
    if (error != null)
    {
      Notify("Ungoogled Chromium update failed", error);
    }
  }

  private async Task OfferInstallAsync(string tag)
  {
    if (_notifier == null)
    {
      return;
    }

    string answer = await _notifier.ShowAsync(
      new Toast(
        "Ungoogled Chromium update downloaded",
        $"{tag} is ready. Installing closes Chromium.",
        [new ToastButton(InstallNowId, "Install now"), new ToastButton("later", "Later")]));
    if (answer == InstallNowId)
    {
      await InstallAndReportAsync(WhenRunning.Close);
    }
  }

  private void Notify(string title, string body)
  {
    _ = _notifier?.ShowAsync(new Toast(title, body));
  }

  private static void Report(string message, bool isError)
  {
    GardenContext.UIManager.OKMessageBox(DialogCaption, message, isError ? MessageBoxImage.Warning : MessageBoxImage.Information);
  }

  /// <param name="onDetermined">
  /// Called once it is known whether an update exists, before any download starts. The tray icon already reflects a
  /// download in progress at that point.
  /// </param>
  private async Task CheckOnceAsync(CancellationToken cancellationToken, Action<CheckResult> onDetermined = null)
  {
    await _checkGate.WaitAsync(cancellationToken);
    try
    {
      InstalledChromium installed = InstalledChromium.TryDetect();
      if (installed == null)
      {
        ClearStaged();
        UpdateStaging.Sweep(null);
        onDetermined?.Invoke(new CheckResult(null, null, false));
        return;
      }

      ReleaseInfo release = await ReleaseClient.GetLatestAsync(cancellationToken);
      if (release == null || release.ChromiumVersion <= installed.Version)
      {
        ClearStaged();
        UpdateStaging.Sweep(null);
        onDetermined?.Invoke(new CheckResult(installed, release, false));
        return;
      }

      if (_staged?.Tag == release.Tag && UpdateStaging.IsStillAvailable(_staged.InstallerPath))
      {
        onDetermined?.Invoke(new CheckResult(installed, release, true));
        return;
      }

      SetIconState(IconState.Downloading, installed.ExePath, $"Downloading Ungoogled Chromium {release.Tag}...");
      onDetermined?.Invoke(new CheckResult(installed, release, false));
      string installerPath;
      try
      {
        installerPath = await UpdateStaging.DownloadAsync(release, cancellationToken);
      }
      catch (Exception)
      {
        RestoreIconAfterInterruption(installed);
        throw;
      }

      UpdateStaging.Sweep(installerPath);
      PublishStaged(new StagedUpdate(release.Tag, release.ChromiumVersion, installerPath), installed);
    }
    finally
    {
      _checkGate.Release();
    }
  }

  private void PublishStaged(StagedUpdate staged, InstalledChromium installed)
  {
    _staged = staged;
    SetIconState(IconState.Ready, installed.ExePath, $"Ungoogled Chromium {staged.Tag} is ready - click to install");
  }

  private void ClearStaged()
  {
    if (_staged != null)
    {
      UpdateStaging.Delete(_staged.InstallerPath);
    }

    _staged = null;
    SetIconState(IconState.Hidden, null, null);
  }

  private void RestoreIconAfterInterruption(InstalledChromium installed)
  {
    if (_staged != null)
    {
      SetIconState(IconState.Ready, installed.ExePath, $"Ungoogled Chromium {_staged.Tag} is ready - click to install");
    }
    else
    {
      SetIconState(IconState.Hidden, null, null);
    }
  }

  private void SetIconState(IconState state, string chromeExePath, string tooltip)
  {
    bool visibilityChanged = (_iconState != IconState.Hidden) != (state != IconState.Hidden);
    _iconState = state;
    if (state != IconState.Hidden)
    {
      OnUiThread(
        () =>
        {
          if (_notifyIcon == null)
          {
            return;
          }

          Icon previous = _notifyIcon.Icon;
          _notifyIcon.Icon = UpdateIconFactory.Create(chromeExePath, ToIconKind(state));
          previous?.Dispose();
          _notifyIcon.Text = Truncate(tooltip);
        });
    }

    if (visibilityChanged)
    {
      IsIconVisibleChanged?.Invoke(this, EventArgs.Empty);
    }
  }

  private static UpdateIconKind ToIconKind(IconState state)
  {
    return state switch
    {
      IconState.Downloading => UpdateIconKind.Downloading,
      IconState.Installing => UpdateIconKind.Installing,
      _ => UpdateIconKind.UpdateReady
    };
  }

  private void OnIconMouseClick(object sender, MouseEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
    {
      _ = InstallAndReportAsync(WhenRunning.Ask);
    }
  }

  private ContextMenuStrip CreateContextMenu()
  {
    var menu = new ContextMenuStrip();
    menu.Items.Add("Install update", null, (_, _) => _ = InstallAndReportAsync(WhenRunning.Ask));
    menu.Items.Add("Check now", null, (_, _) => _ = CheckNowAsync());
    return menu;
  }

  /// <returns>An error message, or null when the update succeeded, was declined or there was nothing to install.</returns>
  private async Task<string> InstallAsync(WhenRunning whenRunning)
  {
    StagedUpdate staged = _staged;
    if (staged == null || _installing || _iconState != IconState.Ready)
    {
      return null;
    }

    _installing = true;
    InstalledChromium installed = null;
    try
    {
      installed = InstalledChromium.TryDetect();
      if (installed == null)
      {
        return "Ungoogled Chromium installation was not found.";
      }

      if (!File.Exists(staged.InstallerPath))
      {
        return "The downloaded installer is missing. Run the update check again.";
      }

      bool wasRunning = ChromiumCloser.IsRunning(installed);
      if (wasRunning && whenRunning == WhenRunning.Skip)
      {
        return null;
      }

      if (wasRunning && whenRunning == WhenRunning.Ask)
      {
        DialogResult answer = MessageBox.Show(
          $"Chromium is running. Close it and install {staged.Tag}?",
          DialogCaption,
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question);
        if (answer != DialogResult.Yes)
        {
          return null;
        }
      }

      SetIconState(IconState.Installing, installed.ExePath, $"Installing Ungoogled Chromium {staged.Tag}...");
      if (wasRunning)
      {
        if (!await ChromiumCloser.ExitGracefullyAsync(installed, ExitTimeout))
        {
          return "Chromium did not exit (a page may be asking for confirmation). Close it manually and try again.";
        }
      }

      using (Process installer = Process.Start(new ProcessStartInfo(staged.InstallerPath) { UseShellExecute = true }))
      {
        if (installer != null)
        {
          await installer.WaitForExitAsync();
        }
      }

      InstalledChromium updated = InstalledChromium.TryDetect();
      if (updated == null || updated.Version < staged.ChromiumVersion)
      {
        return $"The installer finished, but Chromium is still at {updated?.Version}, expected {staged.ChromiumVersion}. The installer may have been cancelled or failed.";
      }

      ClearStaged();
      if (wasRunning)
      {
        await StartAgainIfNotRunningAsync(updated);
      }

      return null;
    }
    catch (Exception exception)
    {
      Log.For(this).Warning(exception, "Ungoogled Chromium install failed");
      return $"Install failed: {exception.Message}";
    }
    finally
    {
      _installing = false;
      if (installed != null && _iconState == IconState.Installing)
      {
        RestoreIconAfterInterruption(installed);
      }
    }
  }

  /// <summary>
  /// The Chromium installer normally starts the browser itself, so a second instance is started only when none appears.
  /// </summary>
  private static async Task StartAgainIfNotRunningAsync(InstalledChromium installed)
  {
    for (int attempt = 0; attempt < 20; attempt++)
    {
      if (ChromiumCloser.IsRunning(installed))
      {
        return;
      }

      await Task.Delay(500);
    }

    Process.Start(new ProcessStartInfo(installed.ExePath) { UseShellExecute = true })?.Dispose();
  }

  private static string Truncate(string text)
  {
    // NotifyIcon.Text is limited to 127 characters.
    return text.Length <= 127 ? text : text[..127];
  }

  private static void OnUiThread(Action action)
  {
    Application.Current?.Dispatcher.BeginInvoke(action);
  }

  private enum WhenRunning
  {
    Skip,
    Ask,
    Close
  }

  private enum IconState
  {
    Hidden,
    Downloading,
    Ready,
    Installing
  }

  private sealed record StagedUpdate(string Tag, Version ChromiumVersion, string InstallerPath);

  private sealed record CheckResult(InstalledChromium Installed, ReleaseInfo Release, bool AlreadyDownloaded)
  {
    public bool UpdateAvailable => Installed != null && Release != null && Release.ChromiumVersion > Installed.Version;

    public string Describe()
    {
      if (Installed == null)
      {
        return "Ungoogled Chromium installation was not found.";
      }

      if (Release == null)
      {
        return "No installer for this machine was found in the latest release.";
      }

      if (!UpdateAvailable)
      {
        return $"Ungoogled Chromium is up to date.{Environment.NewLine}(installed {Installed.Version}, latest {Release.ChromiumVersion})";
      }

      string next = AlreadyDownloaded
        ? "It is already downloaded: click the tray icon to install it."
        : "It is being downloaded: watch the tray icon, then click it to install.";
      return $"Update available: {Release.Tag}.{Environment.NewLine}(installed {Installed.Version}){Environment.NewLine}{next}";
    }
  }
}
