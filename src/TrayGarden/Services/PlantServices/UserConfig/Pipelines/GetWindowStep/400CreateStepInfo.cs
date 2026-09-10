using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;

[UsedImplicitly]
public class CreateStepInfo : IPipelineProcessor<GetUCStepPipelineArgs>
{
  public string GlobalTitle { get; set; } = "Tray Garden -- User settings for #plantName plant";

  public string Header { get; set; } = "User settings for plant";

  public string ShortName { get; set; } = "user settings";

  [UsedImplicitly]
  public void Process(GetUCStepPipelineArgs args)
  {
    Assert.IsNotNull(args.ConfigurationConstructInfo.ResultControlVM, "args.ConfigurationConstructInfo.ResultControlVM");
    WindowWithBackStateConstructInfo stateInfo = args.StateConstructInfo;
    stateInfo.ResultState = new WindowStepState(
      GetGlobalTitle(args),
      stateInfo.Header ?? Header,
      stateInfo.ShortName ?? ShortName,
      args.ConfigurationConstructInfo.ResultControlVM,
      stateInfo.SuperAction,
      stateInfo.StateSpecificHelpActions);
  }

  private string GetGlobalTitle(GetUCStepPipelineArgs args)
  {
    var globalTitle = args.StateConstructInfo.GlobalTitle ?? GlobalTitle;
    return globalTitle.Replace("#plantName", args.UCServicePlantBox.RelatedPlantEx.Plant.HumanSupportingName);
  }
}
