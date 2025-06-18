using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep
{
  [UsedImplicitly]
  public class AddResetAllHelpAction
  {
    [UsedImplicitly]
    public virtual void Process(GetUCStepPipelineArgs args)
    {
      ConfigurationControlVM contentVM = args.ConfigurationConstructInfo.ResultControlVM;
      Assert.IsNotNull(contentVM, "Content VM can't be null at this stage");
      ICommand command = contentVM.ResetAll;
      WindowWithBackStateConstructInfo stateInfo = args.StateConstructInfo;
      if (stateInfo.StateSpecificHelpActions == null)
      {
        stateInfo.StateSpecificHelpActions = new List<ActionCommandVM>();
      }
      stateInfo.StateSpecificHelpActions.Add(new ActionCommandVM(command, "Reset setting values"));
    }
  }
}