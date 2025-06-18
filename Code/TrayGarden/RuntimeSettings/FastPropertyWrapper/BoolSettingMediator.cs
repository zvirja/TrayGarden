using System;
using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.FastPropertyWrapper;

public class BoolSettingMediator : BaseSettingMediator
{
  public BoolSettingMediator([NotNull] string key, bool defaultValue, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    : base(key, settingsBoxResolver)
  {
    this.DefaultValue = defaultValue;
  }

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
}