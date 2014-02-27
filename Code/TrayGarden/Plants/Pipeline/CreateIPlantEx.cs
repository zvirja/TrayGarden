#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.Plants.Pipeline
{
  [UsedImplicitly]
  public class CreateIPlantEx
  {
    #region Public Methods and Operators

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
        Log.Error("Can't initialize PlantEx", ex, this);
      }
    }

    #endregion
  }
}