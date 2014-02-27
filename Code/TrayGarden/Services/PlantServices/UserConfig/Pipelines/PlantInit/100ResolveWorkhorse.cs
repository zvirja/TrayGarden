#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Reception.Services;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit
{
  [UsedImplicitly]
  public class ResolveWorkhorse
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantUCPipelineArg args)
    {
      var appropriateWorkhorse = args.RelatedPlant.GetFirstWorkhorseOfType<IUserConfiguration>();
      if (appropriateWorkhorse == null)
      {
        args.Abort();
        return;
      }
      args.Workhorse = appropriateWorkhorse;
    }

    #endregion
  }
}