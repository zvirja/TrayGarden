using System;
using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper;

public class DoubleSettingMediator : BaseSettingMediator
{
  public DoubleSettingMediator([NotNull] string key, double defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    : base(key, settingsBoxResolver)
  {
    DefaultValue = defaultValue;
  }

  public double DefaultValue { get; set; }

  public double Value
  {
    get
    {
      return SettingsBox.GetDouble(Key, DefaultValue);
    }
    set
    {
      SettingsBox.SetDouble(Key, value);
    }
  }
}