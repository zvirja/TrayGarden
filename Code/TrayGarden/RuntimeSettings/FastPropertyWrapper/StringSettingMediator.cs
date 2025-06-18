using System;
using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper;

public class StringSettingMediator : BaseSettingMediator
{
  public StringSettingMediator([NotNull] string key, string defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    : base(key, settingsBoxResolver)
  {
    DefaultValue = defaultValue;
  }

  public string DefaultValue { get; set; }

  public string Value
  {
    get
    {
      return SettingsBox.GetString(Key, DefaultValue);
    }
    set
    {
      SettingsBox.SetString(Key, value);
    }
  }
}