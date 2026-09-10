using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ResolveIPlant : IPipelineProcessor<InitializePlantArgs>
{
  [UsedImplicitly]
  public void Process(InitializePlantArgs args)
  {
    var iPlant = args.PlantObject as IPlant;
    if (iPlant != null)
    {
      args.IPlantObject = iPlant;
    }
    else
    {
      args.Abort();
    }
  }
}
