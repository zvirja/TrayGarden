using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

public class ResolveSinglePlantVMPipelineArgs : PipelineArgs
{
  public ResolveSinglePlantVMPipelineArgs([NotNull] IPlantEx plantEx)
  {
    Assert.ArgumentNotNull(plantEx, "plantEx");
    this.PlantEx = plantEx;
  }

  public IPlantEx PlantEx { get; set; }

  public SinglePlantVM PlantVM { get; set; }
}