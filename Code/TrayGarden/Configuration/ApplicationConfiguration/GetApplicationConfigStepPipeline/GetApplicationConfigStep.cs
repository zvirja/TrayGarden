using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
    public static class GetApplicationConfigStep
    {
        public static void Run(GetApplicationConfigStepArgs args)
        {
            HatcherGuide<IPipelineManager>.Instance.InvokePipeline("getApplicationConfigStep", args);
        }
    }
}
