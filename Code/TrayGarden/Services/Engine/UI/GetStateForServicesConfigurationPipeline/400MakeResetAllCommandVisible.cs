using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

[UsedImplicitly]
public class MakeResetAllCommandVisible
{
  [UsedImplicitly]
  public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
  {
    Assert.IsNotNull(args.ConfigConstructInfo.ResultControlVM, "args.ConfigConstructInfo.ResultControlVM");
    var resetAllCommand = new ActionCommandVM(args.ConfigConstructInfo.ResultControlVM.ResetAll, "Restore to actual values");
    WindowWithBackStateConstructInfo windowWithBackStateConstructInfo = args.StateConstructInfo;
    if (windowWithBackStateConstructInfo.StateSpecificHelpActions == null)
    {
      windowWithBackStateConstructInfo.StateSpecificHelpActions = new List<ActionCommandVM> { resetAllCommand };
    }
    else
    {
      windowWithBackStateConstructInfo.StateSpecificHelpActions.Add(resetAllCommand);
    }
  }
}