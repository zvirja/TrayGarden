﻿using System.Collections.Generic;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.RareCommands.Core;

namespace TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit;

public class InitPlantRareCommandsArgs : PipelineArgs
{
  public InitPlantRareCommandsArgs(IPlantEx relatedPlant)
  {
    RelatedPlant = relatedPlant;
  }

  public List<IRareCommand> CollectedCommands { get; set; }

  public IPlantEx RelatedPlant { get; set; }
}