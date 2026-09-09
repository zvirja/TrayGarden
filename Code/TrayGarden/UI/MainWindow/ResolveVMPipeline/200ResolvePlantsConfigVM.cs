using System.Collections.Generic;
using System.Collections.ObjectModel;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline;

public class ResolvePlantsConfigVM(IGardenbed gardenbed, IPipelineRunner pipelineRunner)
  : IPipelineProcessor<GetMainVMPipelineArgs>
{
  [UsedImplicitly]
  public void Process(GetMainVMPipelineArgs args)
  {
    var plantsConfig = new PlantsConfigVM();
    plantsConfig.PlantVMs = new ObservableCollection<SinglePlantVM>(GetSinglePlantVMs());
    args.PlantsConfigVM = plantsConfig;
  }

  private SinglePlantVM GetSinglePlantVM(IPlantEx plantEx)
  {
    var pipelineArgs = new ResolveSinglePlantVMPipelineArgs(plantEx);
    pipelineRunner.Run(pipelineArgs);
    return pipelineArgs.Aborted ? null : pipelineArgs.PlantVM;
  }

  private List<SinglePlantVM> GetSinglePlantVMs()
  {
    var result = new List<SinglePlantVM>();
    var plantExAll = gardenbed.GetAllPlants();
    foreach (IPlantEx plantEx in plantExAll)
    {
      var resolvedPlantVM = GetSinglePlantVM(plantEx);
      if (resolvedPlantVM == null)
      {
        Log.For(this).Warning("VM for plant wasn't resolved. Plant type: {PlantType}", plantEx.Plant.GetType());
      }
      else
      {
        result.Add(resolvedPlantVM);
      }
    }

    return result;
  }
}
