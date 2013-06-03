using TrayGarden.Pipelines.Engine;
using TrayGarden.Pipelines.Shutdown;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Startup
{
    public class StartupPipeline
    {
        public static void Run()
        {
            var args = new StartupArgs();
            HatcherGuide<IPipelineManager>.Instance.InvokePipelineUnmaskedExceptions("startup", args);
        }
    }
}
