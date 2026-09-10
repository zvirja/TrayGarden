using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class CreateStep : IPipelineProcessor<UNConfigurationStepArgs>
{
  [UsedImplicitly]
  public void Process(UNConfigurationStepArgs args)
  {
    var constructInfo = args.StateConstructInfo;
    constructInfo.ResultState = new WindowStepState(
      constructInfo.GlobalTitle,
      constructInfo.Header,
      constructInfo.ShortName,
      args.ConfigurationConstructInfo.ResultControlVM,
      constructInfo.SuperAction,
      constructInfo.StateSpecificHelpActions);
  }
}
