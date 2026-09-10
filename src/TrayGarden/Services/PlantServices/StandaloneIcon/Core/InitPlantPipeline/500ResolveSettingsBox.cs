using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

[UsedImplicitly]
public class ResolveSettingsBox : IPipelineProcessor<InitPlantSIArgs>
{
  [UsedImplicitly]
  public void Process(InitPlantSIArgs args)
  {
    ISettingsBox settingsBox = args.SIBox.RelatedPlantEx.MySettingsBox.GetSubBox("StandaloneIconService");
    args.SIBox.SettingsBox = settingsBox;
  }
}
