using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

[UsedImplicitly]
public class CreatePlantVM : IPipelineProcessor<ResolveSinglePlantVMPipelineArgs>
{
  [UsedImplicitly]
  public void Process(ResolveSinglePlantVMPipelineArgs args)
  {
    args.PlantVM = new SinglePlantVM();
    args.PlantVM.InitPlantVMWithPlantEx(args.PlantEx);
  }
}
