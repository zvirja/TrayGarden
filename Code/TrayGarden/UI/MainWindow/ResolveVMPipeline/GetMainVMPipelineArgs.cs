using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline
{
  public class GetMainVMPipelineArgs : PipelineArgs
  {
    public GetMainVMPipelineArgs()
    {
      this.SuperAction = null;
      this.StateSpecificHelpActions = new List<ActionCommandVM>();
    }

    public PlantsConfigVM PlantsConfigVM { get; set; }

    public WindowWithBackVM ResultVM { get; set; }

    public List<ActionCommandVM> StateSpecificHelpActions { get; set; }

    public ActionCommandVM SuperAction { get; set; }
  }
}