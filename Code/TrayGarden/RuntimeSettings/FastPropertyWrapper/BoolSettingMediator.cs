using System;
using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper;

public class BoolSettingMediator : BaseSettingMediator
{
  public BoolSettingMediator([NotNull] string key, bool defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    : base(key, settingsBoxResolver)
  {
    DefaultValue = defaultValue;
  }

  public bool DefaultValue { get; set; }

  public bool Value
  {
    get
    {
      return SettingsBox.GetBool(Key, DefaultValue);
    }
    set
    {
      SettingsBox.SetBool(Key, value);
    }
  }
}