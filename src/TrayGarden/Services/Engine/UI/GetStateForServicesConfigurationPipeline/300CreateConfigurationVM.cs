using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

[UsedImplicitly]
public class CreateConfigurationVM : IPipelineProcessor<GetStateForServicesConfigurationPipelineArgs>
{
  public string ConfigurationDescription { get; set; } =
    "This window allows to enable or disable the particular plant service. Pay attention that some services cannot be disabled. You have to restart application to apply changes";

  [UsedImplicitly]
  public void Process([NotNull] GetStateForServicesConfigurationPipelineArgs args)
  {
    ConfigurationControlConstructInfo configConstructInfo = args.ConfigConstructInfo;
    Assert.ArgumentNotNull(configConstructInfo.ConfigurationEntries, "args.ConfigConstructInfo.ConfigurationEntries");
    configConstructInfo.ResultControlVM = new ConfigurationControlVM(
      configConstructInfo.ConfigurationEntries,
      configConstructInfo.EnableResetAllOption)
    {
      ConfigurationDescription = ConfigurationDescription,
      CalculateRebootOption = configConstructInfo.AllowReboot
    };
  }
}
