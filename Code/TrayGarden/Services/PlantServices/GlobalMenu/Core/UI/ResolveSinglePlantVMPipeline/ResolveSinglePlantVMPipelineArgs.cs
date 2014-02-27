#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
  public class ResolveSinglePlantVMPipelineArgs : PipelineArgs
  {
    #region Constructors and Destructors

    public ResolveSinglePlantVMPipelineArgs([NotNull] IPlantEx plantEx)
    {
      Assert.ArgumentNotNull(plantEx, "plantEx");
      this.PlantEx = plantEx;
    }

    #endregion

    #region Public Properties

    public IPlantEx PlantEx { get; set; }

    public SinglePlantVM PlantVM { get; set; }

    #endregion
  }
}