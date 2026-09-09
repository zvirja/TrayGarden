using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;

[UsedImplicitly]
public class CreateSettingsBox : IPipelineProcessor<InitPlantGMArgs>
{
  [UsedImplicitly]
  public void Process(InitPlantGMArgs args)
  {
    var settingsBox = args.PlantEx.MySettingsBox.GetSubBox("GlobalMenuService");
    args.GMBox.SettingsBox = settingsBox;
  }
}
