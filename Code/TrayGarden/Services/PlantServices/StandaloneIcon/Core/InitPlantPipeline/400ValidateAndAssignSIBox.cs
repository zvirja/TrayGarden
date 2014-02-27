#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class ValidateAndAssignSIBox
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantSIArgs args)
    {
      if (!this.IsSIBoxValid(args.SIBox))
      {
        args.Abort();
        return;
      }
      args.SIBox.RelatedPlantEx = args.PlantEx;
      args.PlantEx.PutLuggage(args.LuggageName, args.SIBox);
    }

    #endregion

    #region Methods

    protected virtual bool IsSIBoxValid(StandaloneIconPlantBox box)
    {
      if (box == null)
      {
        return false;
      }
      if (box.NotifyIcon == null)
      {
        return false;
      }
      return true;
    }

    #endregion
  }
}