using System;
using JetBrains.Annotations;

using TrayGarden.RuntimeSettings;
using TrayGarden.RuntimeSettings.FastPropertyWrapper;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration;

public class TimeSpanSettingMediator : BaseSettingMediator
{
  public TimeSpanSettingMediator([NotNull] string key, int defaultMilliseconds, [NotNull] Func<ISettingsBox> settingsBoxResolver)
    : base(key, settingsBoxResolver)
  {
    DefaultMillisecondsValue = defaultMilliseconds;
  }

  public int DefaultMillisecondsValue { get; set; }

  public TimeSpan Value
  {
    get
    {
      return TimeSpan.FromMilliseconds(SettingsBox.GetInt(Key, DefaultMillisecondsValue));
    }
    set
    {
      SettingsBox.SetInt(Key, (int)value.TotalMilliseconds);
    }
  }
}