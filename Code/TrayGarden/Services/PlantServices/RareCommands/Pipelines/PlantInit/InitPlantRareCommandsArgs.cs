using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.RareCommands.Core;

namespace TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit
{
  public class InitPlantRareCommandsArgs : PipelineArgs
  {
    public InitPlantRareCommandsArgs(IPlantEx relatedPlant)
    {
      this.RelatedPlant = relatedPlant;
    }

    public List<IRareCommand> CollectedCommands { get; set; }

    public IPlantEx RelatedPlant { get; set; }
  }
}