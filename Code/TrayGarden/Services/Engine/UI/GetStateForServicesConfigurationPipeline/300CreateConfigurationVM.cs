using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

[UsedImplicitly]
public class CreateConfigurationVM
{
  public CreateConfigurationVM()
  {
    this.ConfigurationDescription =
      "This window allows to enable or disable the particular plant service. Pay attention that some services cannot be disabled. You have to restart application to apply changes";
  }

  public string ConfigurationDescription { get; set; }

  [UsedImplicitly]
  public virtual void Process([NotNull] GetStateForServicesConfigurationPipelineArgs args)
  {
    ConfigurationControlConstructInfo configConstructInfo = args.ConfigConstructInfo;
    Assert.ArgumentNotNull(configConstructInfo.ConfigurationEntries, "args.ConfigConstructInfo.ConfigurationEntries");
    configConstructInfo.ResultControlVM = new ConfigurationControlVM(
      configConstructInfo.ConfigurationEntries,
      configConstructInfo.EnableResetAllOption)
    {
      ConfigurationDescription = this.ConfigurationDescription,
      CalculateRebootOption = configConstructInfo.AllowReboot
    };
  }
}