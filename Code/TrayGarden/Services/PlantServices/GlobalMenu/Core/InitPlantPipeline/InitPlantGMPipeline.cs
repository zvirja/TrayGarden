using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  public static class InitPlantGMPipeline
  {
    public static InitPlantGMArgs Run(IPlantEx plantEx, string luggageName, INotifyIconChangerMaster globalNotifyIconChanger)
    {
      var args = new InitPlantGMArgs(plantEx, luggageName, globalNotifyIconChanger);
      HatcherGuide<IPipelineManager>.Instance.InvokePipeline("globalMenuServiceInitPlant", args);
      return args;
    }
  }
}