using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UI.GetUCStepPipeline
{
    public static class GetUCStepPipelineRunner
    {
        public static void Run(GetUCStepPipelineArgs args)
        {
            HatcherGuide<IPipelineManager>.Instance.InvokePipeline("getWindowStepForUserSettingsVisual", args);
        }
    }
}
