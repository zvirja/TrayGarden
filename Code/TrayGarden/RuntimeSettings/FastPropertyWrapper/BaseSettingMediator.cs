using System;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper;

public abstract class BaseSettingMediator
{
  protected BaseSettingMediator([NotNull] string key, [NotNull] Func<ISettingsBox> settingsBoxResolver)
  {
    Assert.ArgumentNotNullOrEmpty(key, "key");
    Assert.ArgumentNotNull(settingsBoxResolver, "settingsBoxResolver");
    SettingsBoxResolver = settingsBoxResolver;
    Key = key;
  }

  public string Key { get; set; }

  protected ISettingsBox SettingsBox
  {
    get
    {
      ISettingsBox resolvedBox = SettingsBoxResolver();
      Assert.IsNotNull(resolvedBox, "Box cannot be null");
      return resolvedBox;
    }
  }

  protected Func<ISettingsBox> SettingsBoxResolver { get; set; }
}