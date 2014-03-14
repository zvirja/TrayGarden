#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Reception.Services;

#endregion

namespace TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit
{
  public class CollectRareCommands
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantRareCommandsArgs args)
    {
      var workHorse = args.RelatedPlant.GetFirstWorkhorseOfType<IProvidesRareCommands>();
      if (workHorse == null)
      {
        args.Abort();
        return;
      }
      args.CollectedCommands = workHorse.GetRareCommands();
    }

    #endregion
  }
}