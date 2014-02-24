using System;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
    public class ResolveSinglePlantVMPipelineArgs: PipelineArgs
    {
        public SinglePlantVM PlantVM { get; set; }
        public IPlantEx PlantEx { get; set; }

        public ResolveSinglePlantVMPipelineArgs([NotNull] IPlantEx plantEx)
        {
            Assert.ArgumentNotNull(plantEx, "plantEx");
            PlantEx = plantEx;
        }
    }
}
