using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;

public static class GetApplicationConfigStep
{
  public static void Run(GetApplicationConfigStepArgs args)
  {
    HatcherGuide<IPipelineManager>.Instance.InvokePipeline("getApplicationConfigStep", args);
  }
}