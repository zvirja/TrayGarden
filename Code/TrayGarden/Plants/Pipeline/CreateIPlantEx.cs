using System;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class CreateIPlantEx : IPipelineProcessor<InitializePlantArgs>
{
  [UsedImplicitly]
  public virtual void Process(InitializePlantArgs args)
  {
    var plantEx = new PlantEx();
    try
    {
      plantEx.Initialize(args.IPlantObject, args.Workhorses, args.PlantID, args.PlantSettingsBox);
      args.ResolvedPlantEx = plantEx;
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Can't initialize PlantEx");
    }
  }
}
