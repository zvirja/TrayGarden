using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper
{
  public class BaseSettingMediator
  {
    protected Func<ISettingsBox> SettingsBoxResolver { get; set; }
    protected ISettingsBox SettingsBox
    {
      get
      {
        ISettingsBox resolvedBox = SettingsBoxResolver();
        Assert.IsNotNull(resolvedBox, "Box cannot be null");
        return resolvedBox;
      }
    }

    public string Key { get; set; }

    protected BaseSettingMediator([NotNull] string key, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    {
      Assert.ArgumentNotNullOrEmpty(key, "key");
      Assert.ArgumentNotNull(settingsBoxResolver, "settingsBoxResolver");
      SettingsBoxResolver = settingsBoxResolver;
      Key = key;
    }
  }
}
