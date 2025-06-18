﻿using System.Timers;

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

  protected ISettingsBox RootBox { get; set; }

  protected IContainer RootContainer { get; set; }

  protected ISettingsStorage SettingsStorage { get; set; }

  protected Timer TimerForAutosave { get; set; }

  [UsedImplicitly]
  public virtual void Initialize(ISettingsStorage settingsStorage)
  {
    Assert.ArgumentNotNull(settingsStorage, "settingsStorage");
    SettingsStorage = settingsStorage;
    SettingsStorage.LoadSettings();
    RootContainer = SettingsStorage.GetRootContainer();
    RootBox = GetRootBox(RootContainer);
    var autosaveInterval = AutoSaveInterval;
    if (autosaveInterval > 0)
    {
      TimerForAutosave = new Timer(autosaveInterval * 1000);
      TimerForAutosave.Elapsed += TimerForAutosave_Elapsed;
      TimerForAutosave.Enabled = true;
    }
  }

  public virtual bool SaveNow(bool force)
  {
    return SaveSettingsInternal(force);
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
      return SettingsStorage.SaveSettings();
    }
  }

  protected virtual void TimerForAutosave_Elapsed(object sender, ElapsedEventArgs e)
  {
    SaveSettingsInternal(true);
  }
}