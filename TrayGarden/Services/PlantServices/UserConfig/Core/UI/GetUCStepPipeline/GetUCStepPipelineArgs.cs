using System.Collections.Generic;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.Configuration.Stuff;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UI.GetUCStepPipeline
{
    public class GetUCStepPipelineArgs:PipelineArgs
    {
        public WindowWithBackStateConstructInfo StateConstructInfo { get; set; }
        public ConfigurationControlConstructInfo ConfigurationConstructInfo { get; set; }
        public UserConfigServicePlantBox UCServicePlantBox { get; set; }

        public GetUCStepPipelineArgs(UserConfigServicePlantBox ucServicePlantBox)
        {
            UCServicePlantBox = ucServicePlantBox;
            StateConstructInfo = new WindowWithBackStateConstructInfo();
            ConfigurationConstructInfo = new ConfigurationControlConstructInfo();
        }
    }
}
