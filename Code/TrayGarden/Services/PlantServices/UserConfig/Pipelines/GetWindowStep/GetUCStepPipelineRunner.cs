#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep
{
  public static class GetUCStepPipelineRunner
  {
    #region Public Methods and Operators

    public static void Run(GetUCStepPipelineArgs args)
    {
      HatcherGuide<IPipelineManager>.Instance.InvokePipeline("getWindowStepForUserSettingsVisual", args);
    }

    #endregion
  }
}