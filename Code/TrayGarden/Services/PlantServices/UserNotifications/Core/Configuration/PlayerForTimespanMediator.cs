using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration
{
  public class PlayerForTimespanMediator : TypedConfigurationPlayer<int>
  {
    public PlayerForTimespanMediator([NotNull] string settingName, TimeSpanSettingMediator mediator)
      : base(settingName, true, false)
    {
      this.Mediator = mediator;
    }

    public override int Value
    {
      get
      {
        return (int)this.Mediator.Value.TotalMilliseconds;
      }
      set
      {
        this.Mediator.Value = TimeSpan.FromMilliseconds(value);
      }
    }

    protected TimeSpanSettingMediator Mediator { get; set; }

    public override void Reset()
    {
      this.Mediator.Value = TimeSpan.FromMilliseconds(this.Mediator.DefaultMillisecondsValue);
      this.OnValueChanged();
    }
  }
}