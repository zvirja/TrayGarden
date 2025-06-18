using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper
{
  public class DoubleSettingMediator : BaseSettingMediator
  {
    public DoubleSettingMediator([NotNull] string key, double defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
      : base(key, settingsBoxResolver)
    {
      this.DefaultValue = defaultValue;
    }

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
  }
}