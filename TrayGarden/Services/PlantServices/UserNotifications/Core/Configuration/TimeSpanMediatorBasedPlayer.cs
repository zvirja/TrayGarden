using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration
{
  public class TimeSpanMediatorBasedPlayer : ConfigurationAwarePlayer
  {
    protected TimeSpanSettingMediator Mediator { get; set; }
    public override int IntValue
    {
      get { return (int) Mediator.Value.TotalMilliseconds; }
      set { Mediator.Value = TimeSpan.FromMilliseconds(value); }
    }

    public TimeSpanMediatorBasedPlayer([NotNull] string settingName, TimeSpanSettingMediator mediator) : base(settingName, true, false)
    {
      Mediator = mediator;
    }

    public override void Reset()
    {
      base.Reset();
      Mediator.Value = TimeSpan.FromMilliseconds(Mediator.DefaultMillisecondsValue);
      OnValueChanged();
    }
  }
}
