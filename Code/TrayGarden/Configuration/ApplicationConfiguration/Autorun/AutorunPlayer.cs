using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.Configuration.ApplicationConfiguration.Autorun;

public class AutorunPlayer : TypedConfigurationPlayer<bool>
{
  private readonly IAutorunHelper _autorunHelper;

  public AutorunPlayer(IAutorunHelper autorunHelper, [NotNull] string settingName, string settingDescription)
    : base(settingName, false, false)
  {
    _autorunHelper = autorunHelper;
    base.SettingDescription = settingDescription;
  }

  public override bool Value
  {
    get
    {
      return _autorunHelper.IsAddedToAutorun;
    }
    set
    {
      _autorunHelper.SetNewAutorunValue(value);
      OnValueChanged();
    }
  }

  public override void Reset()
  {
  }
}
