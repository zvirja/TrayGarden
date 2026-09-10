using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;

public class GetUCStepPipelineArgs : PipelineArgs
{
  public GetUCStepPipelineArgs(UserConfigServicePlantBox ucServicePlantBox)
  {
    UCServicePlantBox = ucServicePlantBox;
    StateConstructInfo = new WindowWithBackStateConstructInfo();
    ConfigurationConstructInfo = new ConfigurationControlConstructInfo();
  }

  public ConfigurationControlConstructInfo ConfigurationConstructInfo { get; set; }

  public WindowWithBackStateConstructInfo StateConstructInfo { get; set; }

  public UserConfigServicePlantBox UCServicePlantBox { get; set; }
}