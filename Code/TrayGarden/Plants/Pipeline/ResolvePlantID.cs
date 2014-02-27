#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Plants.Pipeline
{
  [UsedImplicitly]
  public class ResolvePlantID
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitializePlantArgs args)
    {
      args.PlantID = args.PlantObject.GetType().FullName;
    }

    #endregion
  }
}