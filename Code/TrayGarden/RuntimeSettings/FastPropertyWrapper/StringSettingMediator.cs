using System;
using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper;

public class StringSettingMediator : BaseSettingMediator
{
  public StringSettingMediator([NotNull] string key, string defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    : base(key, settingsBoxResolver)
  {
    this.DefaultValue = defaultValue;
  }

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
}