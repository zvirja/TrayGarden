using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.Plants.Intergration;

public class AutoLoadPropertyPlayer : TypedConfigurationPlayer<bool>
{
  private readonly IGardenbed _gardenbed;

  public AutoLoadPropertyPlayer(IGardenbed gardenbed, [NotNull] string settingName, string settingDescription)
    : base(settingName, false, false)
  {
    _gardenbed = gardenbed;
    base.SettingDescription = settingDescription;
  }

  public override bool Value
  {
    get
    {
      return _gardenbed.AutoDetectPlants;
    }
    set
    {
      _gardenbed.AutoDetectPlants = value;
    }
  }

  public override void Reset()
  {
  }
}