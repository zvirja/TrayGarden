using System.Timers;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.RuntimeSettings.Provider;

namespace TrayGarden.RuntimeSettings;

[UsedImplicitly]
public class RuntimeSettingsManager : IRuntimeSettingsManager
{
  protected static readonly object _lock = new object();

  public int AutoSaveInterval { get; set; }

  public virtual ISettingsBox OtherSettings
  {
    get
    {
      return this.RootBox.GetSubBox("other");
    }
  }

  public virtual ISettingsBox SystemSettings
  {
    get
    {
      return this.RootBox.GetSubBox("system");
    }
  }

  protected ISettingsBox RootBox { get; set; }

  protected IContainer RootContainer { get; set; }

  protected ISettingsStorage SettingsStorage { get; set; }

  protected Timer TimerForAutosave { get; set; }

  [UsedImplicitly]
  public virtual void Initialize(ISettingsStorage settingsStorage)
  {
    Assert.ArgumentNotNull(settingsStorage, "settingsStorage");
    this.SettingsStorage = settingsStorage;
    this.SettingsStorage.LoadSettings();
    this.RootContainer = this.SettingsStorage.GetRootContainer();
    this.RootBox = this.GetRootBox(this.RootContainer);
    var autosaveInterval = this.AutoSaveInterval;
    if (autosaveInterval > 0)
    {
      this.TimerForAutosave = new Timer(autosaveInterval * 1000);
      this.TimerForAutosave.Elapsed += this.TimerForAutosave_Elapsed;
      this.TimerForAutosave.Enabled = true;
    }
  }

  public virtual bool SaveNow(bool force)
  {
    return this.SaveSettingsInternal(force);
  }

  protected virtual ISettingsBox GetRootBox(IContainer container)
  {
    var rootBox = new ContainerBasedSettingsBox();
    rootBox.Initialize(container);
    rootBox.OnSaving += this.RootBoxSave;
    return rootBox;
  }

  protected virtual void RootBoxSave()
  {
    this.SaveSettingsInternal(false);
  }

  protected virtual bool SaveSettingsInternal(bool force)
  {
    if (!force && (BulkSettingsUpdate.CurrentValue == BulkUpdateState.Enabled))
    {
      return true;
    }
    lock (_lock)
    {
      return this.SettingsStorage.SaveSettings();
    }
  }

  protected virtual void TimerForAutosave_Elapsed(object sender, ElapsedEventArgs e)
  {
    this.SaveSettingsInternal(true);
  }
}