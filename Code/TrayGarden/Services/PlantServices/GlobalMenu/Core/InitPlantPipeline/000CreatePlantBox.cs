#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class CreatePlantBox
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantGMArgs args)
    {
      args.GMBox = new GlobalMenuPlantBox();
    }

    #endregion
  }
}