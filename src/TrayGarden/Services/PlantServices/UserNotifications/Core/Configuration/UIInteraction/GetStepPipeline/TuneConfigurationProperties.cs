using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class TuneConfigurationProperties : IPipelineProcessor<UNConfigurationStepArgs>
{
  public string ConfigurationDescription { get; set; } = "This window allows to tune the User Nofications service properties";

  [UsedImplicitly]
  public void Process(UNConfigurationStepArgs args)
  {
    ConfigurationControlConstructInfo constructInfo = args.ConfigurationConstructInfo;
    constructInfo.AllowReboot = false;
    constructInfo.EnableResetAllOption = true;
    constructInfo.ConfigurationDescription = ConfigurationDescription;
  }
}
