using JetBrains.Annotations;

using TrayGarden.TypesHatcher;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.Plants.Intergration;

public class AutoLoadPropertyPlayer : TypedConfigurationPlayer<bool>
{
  public AutoLoadPropertyPlayer([NotNull] string settingName, string settingDescription)
    : base(settingName, false, false)
  {
    base.SettingDescription = settingDescription;
  }

  public override bool Value
  {
    get
    {
      return HatcherGuide<IGardenbed>.Instance.AutoDetectPlants;
    }
    set
    {
      HatcherGuide<IGardenbed>.Instance.AutoDetectPlants = value;
    }
  }

  public override void Reset()
  {
  }
}