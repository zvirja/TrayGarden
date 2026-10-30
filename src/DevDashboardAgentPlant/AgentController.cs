using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using DevDashboard.Agent;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TrayGarden.Diagnostics;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.IsEnabledObserver;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.Services.PlantServices.UserNotifications.Core;

using Application = System.Windows.Application;

namespace DevDashboardAgentPlant;

/// <summary>
/// Runs the agent exactly while the plant is enabled. A failed start is reported but leaves the plant enabled; changing
/// a setting or toggling the plant retries it.
/// </summary>
public sealed class AgentController : IIsEnabledObserver,
                                      IProvidesRareCommands,
                                      IClipboardWorks,
                                      IGetPowerOfUserNotifications
{
  private const string ToastTitle = "Dev Dashboard agent";

  private readonly SemaphoreSlim _lifecycleGate = new(1, 1);
  private IPlantEnabledInfo _enabledInfo;
  private IClipboardProvider _clipboard;
  private IUserNotifier _notifier;
  private WebApplication _app;
  private X509Certificate2 _certificate;

  public static AgentController Instance { get; } = new();

  public void ConsumeIsEnabledInfo(IPlantEnabledInfo plantEnabledInfo)
  {
    _enabledInfo = plantEnabledInfo;
    _enabledInfo.IsEnabledChanged += (_, _) => _ = ApplyAsync();
  }

  public void StoreClipboardValueProvider(IClipboardProvider provider)
  {
    _clipboard = provider;
  }

  public void StoreNotifier(IUserNotifier notifier)
  {
    _notifier = notifier;
  }

  public List<IRareCommand> GetRareCommands()
  {
    return
    [
      new SimpleRareCommand(
        "Copy pairing code",
        "Issues a new single-use pairing code (valid for two minutes, replaces the previous one) and copies it to the clipboard. Requires the agent to be running.",
        () => _ = CopyPairingCodeAsync()),
      new SimpleRareCommand(
        "Copy certificate SHA256",
        "Copies the SHA256 fingerprint of the agent's certificate, which the dashboard pins when adding this machine. Requires the agent to be running.",
        CopyCertificateFingerprint)
    ];
  }

  public void Start()
  {
    Application.Current.Exit += (_, _) => ApplyAsync(forceStop: true).GetAwaiter().GetResult();
    foreach (var setting in PlantConfiguration.Instance.RestartingSettings)
    {
      setting.ValueChanged += (_, _) => _ = ApplyAsync(restart: true);
    }

    _ = ApplyAsync();
  }

  private async Task ApplyAsync(bool restart = false, bool forceStop = false)
  {
    await _lifecycleGate.WaitAsync();
    try
    {
      bool shouldRun = !forceStop && (_enabledInfo == null || _enabledInfo.IsEnabled);
      if (!shouldRun || restart)
      {
        await StopAsync();
      }

      if (shouldRun && _app == null)
      {
        await StartAgentAsync();
      }
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Failed to start the Dev Dashboard agent.");
      Notify("Failed to start", ex.Message);
    }
    finally
    {
      _lifecycleGate.Release();
    }
  }

  private async Task StartAgentAsync()
  {
    PlantConfiguration config = PlantConfiguration.Instance;
    X509Certificate2 certificate = AgentCertificate.LoadOrCreate(config.DisplayName.Value);
    WebApplication app = null;
    try
    {
      var address = IPAddress.Parse(config.BindAddress.Value.Trim());
      int port = config.Port.Value;

      var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ContentRootPath = AppContext.BaseDirectory });
      builder.Logging.ClearProviders();
      builder.WebHost.ConfigureKestrel(server => server.Listen(address, port, listen => listen.UseHttps(certificate)));
      builder.Services.AddDevDashboardAgent(new AgentOptions
      {
        AllowedRoots = config.ResolveAllowedRoots(),
        CredentialFile = AgentCertificate.CredentialFile,
        GitPath = string.IsNullOrWhiteSpace(config.GitPath.Value) ? null : config.GitPath.Value.Trim(),
        Deadline = TimeSpan.FromSeconds(config.DeadlineSeconds.Value)
      });

      app = builder.Build();
      app.MapDevDashboardAgent();
      await app.StartAsync();
    }
    catch
    {
      if (app != null)
      {
        await app.DisposeAsync();
      }

      certificate.Dispose();
      throw;
    }

    _app = app;
    _certificate = certificate;
  }

  private async Task StopAsync()
  {
    WebApplication app = _app;
    if (app == null)
    {
      return;
    }

    _app = null;
    try
    {
      using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
      await app.StopAsync(timeout.Token);
      await app.DisposeAsync();
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Failed to stop the Dev Dashboard agent cleanly.");
    }

    _certificate?.Dispose();
    _certificate = null;
  }

  private async Task CopyPairingCodeAsync()
  {
    try
    {
      WebApplication app = _app ?? throw new InvalidOperationException("The agent is not running. Enable the plant first.");
      string code = await app.Services.GetRequiredService<AgentCredentials>().IssuePairingCodeAsync();
      _clipboard.SetCurrentClipboardText(code, silent: true);
      Notify("Pairing code copied", "Valid for two minutes, single use.");
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Failed to copy the pairing code.");
      Notify("Failed to copy the pairing code", ex.Message);
    }
  }

  private void CopyCertificateFingerprint()
  {
    try
    {
      X509Certificate2 certificate = _certificate ?? throw new InvalidOperationException("The agent is not running. Enable the plant first.");
      _clipboard.SetCurrentClipboardText(AgentCertificate.Fingerprint(certificate), silent: true);
      Notify("Certificate SHA256 copied", null);
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Failed to copy the certificate fingerprint.");
      Notify("Failed to copy the certificate SHA256", ex.Message);
    }
  }

  private void Notify(string title, string body)
  {
    _ = _notifier?.ShowAsync(new Toast(ToastTitle + ": " + title, body));
  }
}
