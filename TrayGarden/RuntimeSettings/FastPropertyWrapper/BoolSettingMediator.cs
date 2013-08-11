using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper
{
  public class BoolSettingMediator : BaseSettingMediator
  {
    public bool DefaultValue { get; set; }

    public bool Value
    {
      get { return SettingsBox.GetBool(Key, DefaultValue); }
      set { SettingsBox.GetBool(Key, value); }
    }

    public BoolSettingMediator([NotNull] string key, bool defaultValue,
                                [NotNull] Func<ISettingsBox> settingsBoxResolver)
      : base(key, settingsBoxResolver)
    {
      DefaultValue = defaultValue;
    }
  }
}
