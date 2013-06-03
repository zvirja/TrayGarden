using TrayGarden.Pipelines.Engine;
using TrayGarden.Pipelines.Shutdown;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Startup
{
    public class StartupPipeline
    {
        public static void Run()
        {
            var args = new StartupPipeline();
            HatcherGuide<IPipelineManager>.Instance.InvokePipeline("startup", args);
        }
    }
}
