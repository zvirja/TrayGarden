using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Diagnostics;
using TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline;
using TrayGarden.UI;
using TrayGarden.UI.Common;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.Engine.UI.Intergration
{
  [UsedImplicitly]
  public class ServicesConfigurationInjectSAInjector
  {
    [UsedImplicitly]
    public virtual void Process(GetApplicationConfigStepArgs args)
    {
      args.StepConstructInfo.SuperAction = GetSuperAction();
    }

    protected virtual ActionCommandVM GetSuperAction()
    {
      return new ActionCommandVM(new RelayCommand(ConfigureServices, true), "Configure services");
    }

    protected virtual void ConfigureServices(object o)
    {
      WindowStepState servicesConfigurationState = GetStateFromPipeline();
      Assert.IsNotNull(servicesConfigurationState, "Pipeline hasn't returned state object");
      WindowWithBackVM.GoAheadWithBackIfPossible(servicesConfigurationState);
    }

    protected virtual WindowStepState GetStateFromPipeline()
    {
      return GetStateForServicesConfiguration.Run(new GetStateForServicesConfigurationPipelineArgs());
    }
  }
}
