using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;

namespace TrayGarden.Pipelines.Startup;

[UsedImplicitly]
public class GardenbedInformPostServicesInit(IGardenbed gardenbed) : IPipelineProcessor<StartupArgs>
{
  [UsedImplicitly]
  public void Process(StartupArgs args)
  {
    gardenbed.InformPostInitStage();
  }
}
