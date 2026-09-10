using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ValidatePlant : IPipelineProcessor<InitializePlantArgs>
{
  [UsedImplicitly]
  public void Process(InitializePlantArgs args)
  {
    IPlant plant = args.IPlantObject;
    if (plant.Description.IsNullOrEmpty() || plant.HumanSupportingName.IsNullOrEmpty())
    {
      Log.For(this).Warning("Plant '{PlantID}' doesn't provide correct name and description. It will be disabed", args.PlantID);
      args.Abort();
      args.ResolvedPlantEx = null;
    }
  }
}
