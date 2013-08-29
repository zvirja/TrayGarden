using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline
{
  public class InitPlantUCPipeline
  {

    public static void Run(string luggageName, IPlantEx relatedPlant)
    {
      var args = new InitPlantUCPipelineArg(luggageName, relatedPlant);
      HatcherGuide<IPipelineManager>.Instance.InvokePipeline("userConfigServiceInitPlant", args);
    }
  }
}
