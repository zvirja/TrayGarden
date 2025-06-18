using JetBrains.Annotations;

using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration;

public class PlayerForDoubleRSMediator : TypedConfigurationPlayer<double>
{
  public PlayerForDoubleRSMediator([NotNull] string settingName, DoubleSettingMediator mediator)
    : base(settingName, true, false)
  {
    this.Mediator = mediator;
  }

  public override double Value
  {
    get
    {
      return this.Mediator.Value;
    }
    set
    {
      this.Mediator.Value = value;
    }
  }

  protected DoubleSettingMediator Mediator { get; set; }

  public override void Reset()
  {
    this.Value = this.Mediator.DefaultValue;
    this.OnValueChanged();
  }
}