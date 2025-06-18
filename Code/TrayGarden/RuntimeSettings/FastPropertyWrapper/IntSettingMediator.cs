using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper
{
  public class IntSettingMediator : BaseSettingMediator
  {
    public IntSettingMediator([NotNull] string key, int defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
      : base(key, settingsBoxResolver)
    {
      this.DefaultValue = defaultValue;
    }

    public int DefaultValue { get; set; }

    public int Value
    {
      get
      {
        return this.SettingsBox.GetInt(this.Key, this.DefaultValue);
      }
      set
      {
        this.SettingsBox.SetInt(this.Key, value);
      }
    }
  }
}