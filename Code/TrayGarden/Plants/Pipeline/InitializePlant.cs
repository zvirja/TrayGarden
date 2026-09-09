using System;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Reception;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class InitializePlant
{
  [UsedImplicitly]
  public virtual void Process(InitializePlantArgs args)
  {
    IPlant plant = args.IPlantObject;
    try
    {
      plant.Initialize();
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Unable to initialize plant {PlantType}", plant.GetType());
      args.Abort();
    }
  }
}