using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Integration
{
  [UsedImplicitly]
  public class PlantServiceConfigurator
  {
    [UsedImplicitly]
    public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
    {
      WindowWithBackStateConstructInfo windowWithBackStateConstructInfo = args.StateConstructInfo;
      var command = GetCommand();
      if (windowWithBackStateConstructInfo.StateSpecificHelpActions == null)
      {
        windowWithBackStateConstructInfo.StateSpecificHelpActions = new List<ActionCommandVM> { command };
      }
      else
      {
        windowWithBackStateConstructInfo.StateSpecificHelpActions.Add(command);
      }
    }

    protected virtual ActionCommandVM GetCommand()
    {
      return new ActionCommandVM(new RelayCommand(ShowConfigurationWindow, true), "Configure User Notifications service");
    }

    protected virtual void ShowConfigurationWindow(object obj)
    {
      WindowStepState windowStepState = UNConfigurationStepPipeline.Run();
      WindowWithBackVM.GoAheadWithBackIfPossible(windowStepState);
    }
  }
}
