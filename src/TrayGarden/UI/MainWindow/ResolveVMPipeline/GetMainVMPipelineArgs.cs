using System.Collections.Generic;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline;

public class GetMainVMPipelineArgs : PipelineArgs
{
  public GetMainVMPipelineArgs()
  {
    SuperAction = null;
    StateSpecificHelpActions = new List<ActionCommandVM>();
  }

  public PlantsConfigVM PlantsConfigVM { get; set; }

  public WindowWithBackVM ResultVM { get; set; }

  public List<ActionCommandVM> StateSpecificHelpActions { get; set; }

  public ActionCommandVM SuperAction { get; set; }
}