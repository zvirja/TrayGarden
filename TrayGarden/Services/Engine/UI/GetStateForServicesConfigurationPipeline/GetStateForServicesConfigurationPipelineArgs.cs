using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.Configuration.Stuff;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline
{
    public class GetStateForServicesConfigurationPipelineArgs: PipelineArgs
    {
        public WindowWithBackStateConstructInfo StateConstructInfo { get; set; }
        public ConfigurationControlConstructInfo ConfigConstructInfo { get; set; }

        public GetStateForServicesConfigurationPipelineArgs()
        {
            StateConstructInfo = new WindowWithBackStateConstructInfo();
            ConfigConstructInfo = new ConfigurationControlConstructInfo();
        }
    }
}
