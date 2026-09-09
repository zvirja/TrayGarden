using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

[UsedImplicitly]
public class CreateWindowWithBackState : IPipelineProcessor<GetStateForServicesConfigurationPipelineArgs>
{
  public string GlobalTitle { get; set; } = "Tray Garden -- Services configuration";

  public string Header { get; set; } = "Plant services configuration";

  public string ShortName { get; set; } = "services config";

  [UsedImplicitly]
  public void Process(GetStateForServicesConfigurationPipelineArgs args)
  {
    Assert.IsNotNull(args.ConfigConstructInfo.ResultControlVM, "args.ConfigurationVM");
    WindowWithBackStateConstructInfo stateConstructInfo = args.StateConstructInfo;
    stateConstructInfo.ResultState = new WindowStepState(
      stateConstructInfo.GlobalTitle ?? GlobalTitle,
      stateConstructInfo.Header ?? Header,
      stateConstructInfo.ShortName ?? ShortName,
      args.ConfigConstructInfo.ResultControlVM,
      stateConstructInfo.SuperAction,
      stateConstructInfo.StateSpecificHelpActions);
  }
}
