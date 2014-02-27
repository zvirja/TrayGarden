#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Reception;

#endregion

namespace TrayGarden.Plants.Pipeline
{
  [UsedImplicitly]
  public class InitializePlant
  {
    #region Public Methods and Operators

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

    #endregion
  }
}