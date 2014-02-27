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
  public class ValidatePlant
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitializePlantArgs args)
    {
      IPlant plant = args.IPlantObject;
      if (plant.Description.IsNullOrEmpty() || plant.HumanSupportingName.IsNullOrEmpty())
      {
        Log.Warn("Plant '{0}' doesn't provide correct name and description. It will be disabed".FormatWith(args.PlantID), this);
        args.Abort();
        args.ResolvedPlantEx = null;
      }
    }

    #endregion
  }
}