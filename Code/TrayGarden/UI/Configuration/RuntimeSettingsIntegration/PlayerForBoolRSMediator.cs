using JetBrains.Annotations;

using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration;

public class PlayerForBoolRSMediator : TypedConfigurationPlayer<bool>
{
  public PlayerForBoolRSMediator([NotNull] string settingName, BoolSettingMediator mediator)
    : base(settingName, true, false)
  {
    Mediator = mediator;
  }

  public override bool Value
  {
    get
    {
      return Mediator.Value;
    }
    set
    {
      Mediator.Value = value;
    }
  }

  protected BoolSettingMediator Mediator { get; set; }

  public override void Reset()
  {
    Value = Mediator.DefaultValue;
    OnValueChanged();
  }
}