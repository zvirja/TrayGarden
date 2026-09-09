using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;

[UsedImplicitly]
public class CreatePlantBox : IPipelineProcessor<InitPlantGMArgs>
{
  [UsedImplicitly]
  public void Process(InitPlantGMArgs args)
  {
    args.GMBox = new GlobalMenuPlantBox();
  }
}
