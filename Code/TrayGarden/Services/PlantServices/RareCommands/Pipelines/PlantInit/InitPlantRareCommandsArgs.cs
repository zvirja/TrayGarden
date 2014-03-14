#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.RareCommands.Core;

#endregion

namespace TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit
{
  public class InitPlantRareCommandsArgs : PipelineArgs
  {
    #region Constructors and Destructors

    public InitPlantRareCommandsArgs(IPlantEx relatedPlant)
    {
      this.RelatedPlant = relatedPlant;
    }

    #endregion

    #region Public Properties

    public List<IRareCommand> CollectedCommands { get; set; }

    public IPlantEx RelatedPlant { get; set; }

    #endregion
  }
}