#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper
{
  public abstract class BaseSettingMediator
  {
    #region Constructors and Destructors

    protected BaseSettingMediator([NotNull] string key, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    {
      Assert.ArgumentNotNullOrEmpty(key, "key");
      Assert.ArgumentNotNull(settingsBoxResolver, "settingsBoxResolver");
      this.SettingsBoxResolver = settingsBoxResolver;
      this.Key = key;
    }

    #endregion

    #region Public Properties

    public string Key { get; set; }

    #endregion

    #region Properties

    protected ISettingsBox SettingsBox
    {
      get
      {
        ISettingsBox resolvedBox = this.SettingsBoxResolver();
        Assert.IsNotNull(resolvedBox, "Box cannot be null");
        return resolvedBox;
      }
    }

    protected Func<ISettingsBox> SettingsBoxResolver { get; set; }

    #endregion
  }
}