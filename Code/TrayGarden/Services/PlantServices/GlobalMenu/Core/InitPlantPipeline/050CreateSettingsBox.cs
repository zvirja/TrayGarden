using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;

[UsedImplicitly]
public class CreateSettingsBox
{
  [UsedImplicitly]
  public virtual void Process(InitPlantGMArgs args)
  {
    var settingsBox = args.PlantEx.MySettingsBox.GetSubBox("GlobalMenuService");
    args.GMBox.SettingsBox = settingsBox;
  }
}