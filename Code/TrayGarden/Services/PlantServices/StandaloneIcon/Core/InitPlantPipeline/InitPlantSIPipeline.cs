#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
  public static class InitPlantSIPipeline
  {
    #region Public Methods and Operators

    public static void Run(IPlantEx plantEx, string luggageName, EventHandler closeComponentClick, EventHandler exitGardenClick)
    {
      var args = new InitPlantSIArgs(plantEx, luggageName, closeComponentClick, exitGardenClick);
      HatcherGuide<IPipelineManager>.Instance.InvokePipeline("standaloneIconServiceInitPlant", args);
    }

    #endregion
  }
}