using JetBrains.Annotations;

using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration;

public class PlayerForStringRSMediator : TypedConfigurationPlayer<string>
{
  public PlayerForStringRSMediator([NotNull] string settingName, StringSettingMediator mediator)
    : base(settingName, true, false)
  {
    Mediator = mediator;
  }

  public override string Value
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

  protected StringSettingMediator Mediator { get; set; }

  public override void Reset()
  {
    Value = Mediator.DefaultValue;
    OnValueChanged();
  }
}