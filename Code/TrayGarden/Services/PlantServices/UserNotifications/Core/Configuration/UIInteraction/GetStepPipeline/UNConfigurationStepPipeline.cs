using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

public static class UNConfigurationStepPipeline
{
  public static WindowStepState Run()
  {
    var args = new UNConfigurationStepArgs();
    HatcherGuide<IPipelineManager>.Instance.InvokePipeline("getStateForUserNotificationsUIConfiguration", args);
    return args.StateConstructInfo.ResultState;
  }
}