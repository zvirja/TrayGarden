#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper
{
  public class DoubleSettingMediator : BaseSettingMediator
  {
    #region Constructors and Destructors

    public DoubleSettingMediator([NotNull] string key, double defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
      : base(key, settingsBoxResolver)
    {
      this.DefaultValue = defaultValue;
    }

    #endregion

    #region Public Properties

    public double DefaultValue { get; set; }

    public double Value
    {
      get
      {
        return this.SettingsBox.GetDouble(this.Key, this.DefaultValue);
      }
      set
      {
        this.SettingsBox.SetDouble(this.Key, value);
      }
    }

    #endregion
  }
}