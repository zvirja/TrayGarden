using JetBrains.Annotations;

using TrayGarden.Reception;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ResolveIPlant
{
  [UsedImplicitly]
  public virtual void Process(InitializePlantArgs args)
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