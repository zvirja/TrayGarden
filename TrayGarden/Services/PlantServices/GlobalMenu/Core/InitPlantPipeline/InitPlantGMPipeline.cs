using System;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
    public static class InitPlantGMPipeline
    {
        public static InitPlantGMArgs Run(IPlant plant, string luggageName, INotifyIconChangerClient globalNotifyIconChanger)
        {
            var args = new InitPlantGMArgs(plant, luggageName,globalNotifyIconChanger);
            HatcherGuide<IPipelineManager>.Instance.InvokePipeline("globalMenuServiceInitPlant",args);
            return args;
        }
    }
}
