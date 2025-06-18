using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline;

public static class GetMainVMPipelineRunner
{
  public static WindowWithBackVM Run(GetMainVMPipelineArgs args)
  {
    HatcherGuide<IPipelineManager>.Instance.InvokePipeline("resolveMainWindowVM", args);
    return !args.Aborted ? args.ResultVM : null;
  }
}