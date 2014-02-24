#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper
{
  public class BoolSettingMediator : BaseSettingMediator
  {
    #region Constructors and Destructors

    public BoolSettingMediator([NotNull] string key, bool defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
      : base(key, settingsBoxResolver)
    {
      this.DefaultValue = defaultValue;
    }

    #endregion

    #region Public Properties

    public bool DefaultValue { get; set; }

    public bool Value
    {
      get
      {
        return this.SettingsBox.GetBool(this.Key, this.DefaultValue);
      }
      set
      {
        this.SettingsBox.SetBool(this.Key, value);
      }
    }

    #endregion
  }
}