using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

[UsedImplicitly]
public class CreateWindowWithBackState
{
  public CreateWindowWithBackState()
  {
    this.GlobalTitle = "Tray Garden -- Services configuration";
    this.ShortName = "services config";
    this.Header = "Plant services configuration";
  }

  public string GlobalTitle { get; set; }

  public string Header { get; set; }

  public string ShortName { get; set; }

  [UsedImplicitly]
  public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
  {
    Assert.IsNotNull(args.ConfigConstructInfo.ResultControlVM, "args.ConfigurationVM");
    WindowWithBackStateConstructInfo stateConstructInfo = args.StateConstructInfo;
    stateConstructInfo.ResultState = new WindowStepState(
      stateConstructInfo.GlobalTitle ?? this.GlobalTitle,
      stateConstructInfo.Header ?? this.Header,
      stateConstructInfo.ShortName ?? this.ShortName,
      args.ConfigConstructInfo.ResultControlVM,
      stateConstructInfo.SuperAction,
      stateConstructInfo.StateSpecificHelpActions);
  }
}