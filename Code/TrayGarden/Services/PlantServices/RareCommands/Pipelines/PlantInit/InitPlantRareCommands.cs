#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit
{
  public static class InitPlantRareCommands
  {
    #region Public Methods and Operators

    public static List<IRareCommand> RunPipelineGetCommands(IPlantEx relatedPlant)
    {
      var args = new InitPlantRareCommandsArgs(relatedPlant);
      HatcherGuide<IPipelineManager>.Instance.InvokePipeline("rareCommandsServiceInitPlant", args);
      return args.CollectedCommands;
    }

    #endregion
  }
}