using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

[UsedImplicitly]
public class CreateSIPlantBox : IPipelineProcessor<InitPlantSIArgs>
{
  [UsedImplicitly]
  public void Process(InitPlantSIArgs args)
  {
    args.SIBox = new StandaloneIconPlantBox();
  }
}
