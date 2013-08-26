using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Plants.Pipeline
{
  public static class InitializePlantExPipeline
  {
    public static IPlantEx Run(object plant, ISettingsBox rootSettingsBox)
    {
      var args = new InitializePlantArgs(plant, rootSettingsBox);
      HatcherGuide<IPipelineManager>.Instance.InvokePipeline("initializePlant", args);
      return args.ResolvedPlantEx;
    }
  }
}
