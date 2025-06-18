using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.RuntimeSettings;
using TrayGarden.RuntimeSettings.FastPropertyWrapper;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration
{
  public class TimeSpanSettingMediator : BaseSettingMediator
  {
    public TimeSpanSettingMediator([NotNull] string key, int defaultMilliseconds, [NotNull] Func<ISettingsBox> settingsBoxResolver)
      : base(key, settingsBoxResolver)
    {
      this.DefaultMillisecondsValue = defaultMilliseconds;
    }

    public int DefaultMillisecondsValue { get; set; }

    public TimeSpan Value
    {
      get
      {
        return TimeSpan.FromMilliseconds(this.SettingsBox.GetInt(this.Key, this.DefaultMillisecondsValue));
      }
      set
      {
        this.SettingsBox.SetInt(this.Key, (int)value.TotalMilliseconds);
      }
    }
  }
}