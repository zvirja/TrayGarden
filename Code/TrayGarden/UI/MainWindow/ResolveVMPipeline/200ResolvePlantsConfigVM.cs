using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.TypesHatcher;

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline
{
  public class ResolvePlantsConfigVM
  {
    [UsedImplicitly]
    public virtual void Process(GetMainVMPipelineArgs args)
    {
      var plantsConfig = new PlantsConfigVM();
      plantsConfig.PlantVMs = new ObservableCollection<SinglePlantVM>(this.GetSinglePlantVMs());
      args.PlantsConfigVM = plantsConfig;
    }

    protected virtual SinglePlantVM GetSinglePlantVM(IPlantEx plantEx)
    {
      return ResolveSinglePlantVMPipelineRunner.Run(new ResolveSinglePlantVMPipelineArgs(plantEx));
    }

    protected virtual List<SinglePlantVM> GetSinglePlantVMs()
    {
      var result = new List<SinglePlantVM>();
      var plantExAll = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
      foreach (IPlantEx plantEx in plantExAll)
      {
        var resolvedPlantVM = this.GetSinglePlantVM(plantEx);
        if (resolvedPlantVM == null)
        {
          Log.Warn("VM for plant wasn't resolved. Plant type: {0}".FormatWith(plantEx.Plant.GetType()), this);
        }
        else
        {
          result.Add(resolvedPlantVM);
        }
      }

      return result;
    }
  }
}