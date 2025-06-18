using JetBrains.Annotations;

using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration;

public class PlayerForDoubleRSMediator : TypedConfigurationPlayer<double>
{
  public PlayerForDoubleRSMediator([NotNull] string settingName, DoubleSettingMediator mediator)
    : base(settingName, true, false)
  {
    Mediator = mediator;
  }

  public override double Value
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

  protected DoubleSettingMediator Mediator { get; set; }

  public override void Reset()
  {
    Value = Mediator.DefaultValue;
    OnValueChanged();
  }
}