using System.Timers;

using JetBrains.Annotations;

using Microsoft.Extensions.Options;

using TrayGarden.Configuration.Options;
using TrayGarden.RuntimeSettings.Provider;

namespace TrayGarden.RuntimeSettings;

[UsedImplicitly]
public class RuntimeSettingsManager : IRuntimeSettingsManager
{
  private static readonly object _lock = new object();

  private readonly ISettingsStorage _settingsStorage;

  private readonly int _autoSaveIntervalSeconds;

  private ISettingsBox _rootBox;

  private bool _initialized;

  public RuntimeSettingsManager(ISettingsStorage settingsStorage, IOptions<TrayGardenOptions> options)
  {
    _settingsStorage = settingsStorage;
    _autoSaveIntervalSeconds = options.Value.RuntimeSettings.AutoSaveIntervalSeconds;
  }

  public virtual ISettingsBox OtherSettings
  {
    get
    {
      return RootBox.GetSubBox("other");
    }
  }

  public virtual ISettingsBox SystemSettings
  {
    get
    {
      return RootBox.GetSubBox("system");
    }
  }

  protected ISettingsBox RootBox
  {
    get
    {
      EnsureInitialized();
      return _rootBox;
    }
  }

  protected Timer TimerForAutosave { get; set; }

  public virtual bool SaveNow(bool force)
  {
    return SaveSettingsInternal(force);
  }

  protected virtual void EnsureInitialized()
  {
    if (_initialized)
    {
      return;
    }
    lock (_lock)
    {
      if (_initialized)
      {
        return;
      }
      _settingsStorage.LoadSettings();
      IContainer rootContainer = _settingsStorage.GetRootContainer();
      _rootBox = GetRootBox(rootContainer);
      if (_autoSaveIntervalSeconds > 0)
      {
        TimerForAutosave = new Timer(_autoSaveIntervalSeconds * 1000);
        TimerForAutosave.Elapsed += TimerForAutosave_Elapsed;
        TimerForAutosave.Enabled = true;
      }
      _initialized = true;
    }
  }

  protected virtual ISettingsBox GetRootBox(IContainer container)
  {
    var rootBox = new ContainerBasedSettingsBox();
    rootBox.Initialize(container);
    rootBox.OnSaving += RootBoxSave;
    return rootBox;
  }

  protected virtual void RootBoxSave()
  {
    SaveSettingsInternal(false);
  }

  protected virtual bool SaveSettingsInternal(bool force)
  {
    if (!force && (BulkSettingsUpdate.CurrentValue == BulkUpdateState.Enabled))
    {
      return true;
    }
    lock (_lock)
    {
      return _settingsStorage.SaveSettings();
    }
  }

  protected virtual void TimerForAutosave_Elapsed(object sender, ElapsedEventArgs e)
  {
    SaveSettingsInternal(true);
  }
}
