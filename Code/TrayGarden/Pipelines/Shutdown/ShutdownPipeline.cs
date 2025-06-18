using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Shutdown;

public class ShutdownPipeline
{
  public static void Run()
  {
    var args = new ShutdownArgs();
    HatcherGuide<IPipelineManager>.Instance.InvokePipeline("shutdown", args);
  }
}