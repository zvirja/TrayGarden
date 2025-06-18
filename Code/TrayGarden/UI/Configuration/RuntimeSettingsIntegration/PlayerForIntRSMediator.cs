using JetBrains.Annotations;

using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration;

public class PlayerForIntRSMediator : TypedConfigurationPlayer<int>
{
  public PlayerForIntRSMediator([NotNull] string settingName, IntSettingMediator mediator)
    : base(settingName, true, false)
  {
    Mediator = mediator;
  }

  public override int Value
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

  protected IntSettingMediator Mediator { get; set; }

  public override void Reset()
  {
    Value = Mediator.DefaultValue;
    OnValueChanged();
  }
}