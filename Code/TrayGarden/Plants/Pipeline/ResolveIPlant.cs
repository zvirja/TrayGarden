#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Reception;

#endregion

namespace TrayGarden.Plants.Pipeline
{
  [UsedImplicitly]
  public class ResolveIPlant
  {
    #region Public Methods and Operators

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

    #endregion
  }
}