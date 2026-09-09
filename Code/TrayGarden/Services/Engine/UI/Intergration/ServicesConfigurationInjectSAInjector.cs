using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.Engine.UI.Intergration;

[UsedImplicitly]
public class ServicesConfigurationInjectSAInjector(IPipelineRunner pipelineRunner)
  : IPipelineProcessor<GetApplicationConfigStepArgs>
{
  [UsedImplicitly]
  public void Process(GetApplicationConfigStepArgs args)
  {
    args.StepConstructInfo.SuperAction = GetSuperAction();
  }

  private void ConfigureServices(object o)
  {
    WindowStepState servicesConfigurationState = GetStateFromPipeline();
    Assert.IsNotNull(servicesConfigurationState, "Pipeline hasn't returned state object");
    WindowWithBackVM.GoAheadWithBackIfPossible(servicesConfigurationState);
  }

  private WindowStepState GetStateFromPipeline()
  {
    var pipelineArgs = new GetStateForServicesConfigurationPipelineArgs();
    pipelineRunner.Run(pipelineArgs);
    return pipelineArgs.Aborted ? null : pipelineArgs.StateConstructInfo.ResultState;
  }

  private ActionCommandVM GetSuperAction()
  {
    return new ActionCommandVM(new RelayCommand(ConfigureServices, true), "Configure services");
  }
}
