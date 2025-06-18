using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Startup;

public static class StartupPipeline
{
  public static void Run(string[] startParams)
  {
    var args = new StartupArgs(startParams);
    HatcherGuide<IPipelineManager>.Instance.InvokePipelineUnmaskedExceptions("startup", args);
  }
}