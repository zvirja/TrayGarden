using System;
using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration;

public class PlayerForTimespanMediator : TypedConfigurationPlayer<int>
{
  public PlayerForTimespanMediator([NotNull] string settingName, TimeSpanSettingMediator mediator)
    : base(settingName, true, false)
  {
    Mediator = mediator;
  }

  public override int Value
  {
    get
    {
      return (int)Mediator.Value.TotalMilliseconds;
    }
    set
    {
      Mediator.Value = TimeSpan.FromMilliseconds(value);
    }
  }

  protected TimeSpanSettingMediator Mediator { get; set; }

  public override void Reset()
  {
    Mediator.Value = TimeSpan.FromMilliseconds(Mediator.DefaultMillisecondsValue);
    OnValueChanged();
  }
}