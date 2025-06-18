using System;
using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper;

public class IntSettingMediator : BaseSettingMediator
{
  public IntSettingMediator([NotNull] string key, int defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    : base(key, settingsBoxResolver)
  {
    DefaultValue = defaultValue;
  }

  public int DefaultValue { get; set; }

  public int Value
  {
    get
    {
      return SettingsBox.GetInt(Key, DefaultValue);
    }
    set
    {
      SettingsBox.SetInt(Key, value);
    }
  }
}