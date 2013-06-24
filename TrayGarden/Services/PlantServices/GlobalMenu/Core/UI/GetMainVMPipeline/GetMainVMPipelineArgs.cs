using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline
{
    public class GetMainVMPipelineArgs:PipelineArgs
    {
        public WindowWithBackVMBase ResultVM { get; set; }
        public PlantsConfigVM PlantsConfigVM { get; set; }
    }
}
