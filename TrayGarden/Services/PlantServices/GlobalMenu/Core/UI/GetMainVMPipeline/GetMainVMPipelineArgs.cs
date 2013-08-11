using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline
{
    public class GetMainVMPipelineArgs:PipelineArgs
    {
        public WindowWithBackVM ResultVM { get; set; }
        public PlantsConfigVM PlantsConfigVM { get; set; }
        public ActionCommandVM SuperAction { get; set; }
        public List<ActionCommandVM> StateSpecificHelpActions { get; set; }

        public GetMainVMPipelineArgs()
        {
            SuperAction = null;
            StateSpecificHelpActions = new List<ActionCommandVM>();
        }
    }
}
