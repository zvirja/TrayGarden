using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.TypesHatcher;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline
{
    public class ResolvePlantsConfigVM
    {
        public virtual void Process(GetMainVMPipelineArgs args)
        {
            var plantsConfig = new PlantsConfigVM();
            plantsConfig.PlantVMs = new ObservableCollection<SinglePlantVM>(GetSinglePlantVMs());
            args.PlantsConfigVM = plantsConfig;
        }

        protected virtual List<SinglePlantVM> GetSinglePlantVMs()
        {
            var result = new List<SinglePlantVM>();
            var plantExAll = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
            foreach (IPlantEx plantEx in plantExAll)
            {
                var resolvedPlantVM = GetSinglePlantVM(plantEx);
                if (resolvedPlantVM == null)
                    Log.Warn("VM for plant wasn't resolved. Plant type: {0}".FormatWith(plantEx.Plant.GetType()), this);
                else
                    result.Add(resolvedPlantVM);
            }

            return result;
        }

        protected virtual SinglePlantVM GetSinglePlantVM(IPlantEx plantEx)
        {
            return ResolveSinglePlantVMPipelineRunner.Run(new ResolveSinglePlantVMPipelineArgs(plantEx));
        }
    }
}
