#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper
{
  public class StringSettingMediator : BaseSettingMediator
  {
    #region Constructors and Destructors

    public StringSettingMediator([NotNull] string key, string defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
      : base(key, settingsBoxResolver)
    {
      this.DefaultValue = defaultValue;
    }

    #endregion

    #region Public Properties

    public string DefaultValue { get; set; }

    public string Value
    {
      get
      {
        return this.SettingsBox.GetString(this.Key, this.DefaultValue);
      }
      set
      {
        this.SettingsBox.SetString(this.Key, value);
      }
    }

    #endregion
  }
}