using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Reception;
using TrayGarden.Helpers;

namespace TrayGarden.Plants.Pipeline
{
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
        Log.Error("Unable to initialize plant {0}".FormatWith(plant.GetType()), ex, this);
        args.Abort();
      }
    }
  }
}
