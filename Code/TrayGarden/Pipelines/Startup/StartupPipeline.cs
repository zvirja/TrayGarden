#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Pipelines.Startup
{
  public static class StartupPipeline
  {
    #region Public Methods and Operators

    public static void Run(string[] startParams)
    {
      var args = new StartupArgs(startParams);
      HatcherGuide<IPipelineManager>.Instance.InvokePipelineUnmaskedExceptions("startup", args);
    }

    #endregion
  }
}