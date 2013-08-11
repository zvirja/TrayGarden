using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.RestartApp
{
    public static class RestartAppPipeline
    {
        public static void Run(string[] paramsToAdd)
        {
            HatcherGuide<IPipelineManager>.Instance.InvokePipeline("restartApp", new RestartAppArgs(paramsToAdd));
        }
    }
}
