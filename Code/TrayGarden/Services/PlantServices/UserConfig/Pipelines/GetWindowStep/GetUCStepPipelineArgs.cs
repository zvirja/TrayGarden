using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;

public class GetUCStepPipelineArgs : PipelineArgs
{
  public GetUCStepPipelineArgs(UserConfigServicePlantBox ucServicePlantBox)
  {
    this.UCServicePlantBox = ucServicePlantBox;
    this.StateConstructInfo = new WindowWithBackStateConstructInfo();
    this.ConfigurationConstructInfo = new ConfigurationControlConstructInfo();
  }

  public ConfigurationControlConstructInfo ConfigurationConstructInfo { get; set; }

  public WindowWithBackStateConstructInfo StateConstructInfo { get; set; }

  public UserConfigServicePlantBox UCServicePlantBox { get; set; }
}