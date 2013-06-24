using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline
{
    public class PipelineRunner
    {
        public static WindowWithBackVMBase Run(GetMainVMPipelineArgs args)
        {
            HatcherGuide<IPipelineManager>.Instance.InvokePipeline("globalMenuResolveMainVM", args);
            return !args.Aborted ? args.ResultVM : null;
        }
    }
}
