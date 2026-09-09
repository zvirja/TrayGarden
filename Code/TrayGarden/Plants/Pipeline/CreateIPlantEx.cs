using System;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class CreateIPlantEx
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