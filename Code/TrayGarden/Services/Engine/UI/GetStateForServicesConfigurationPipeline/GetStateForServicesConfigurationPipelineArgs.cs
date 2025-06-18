using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

public class GetStateForServicesConfigurationPipelineArgs : PipelineArgs
{
  public GetStateForServicesConfigurationPipelineArgs()
  {
    StateConstructInfo = new WindowWithBackStateConstructInfo();
    ConfigConstructInfo = new ConfigurationControlConstructInfo();
  }

  public ConfigurationControlConstructInfo ConfigConstructInfo { get; set; }

  public WindowWithBackStateConstructInfo StateConstructInfo { get; set; }
}