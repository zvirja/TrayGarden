using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

public static class ResolveSinglePlantVMPipelineRunner
{
  public static SinglePlantVM Run(ResolveSinglePlantVMPipelineArgs args)
  {
    HatcherGuide<IPipelineManager>.Instance.InvokePipeline("resolveSinglePlantVM", args);
    return !args.Aborted ? args.PlantVM : null;
  }
}