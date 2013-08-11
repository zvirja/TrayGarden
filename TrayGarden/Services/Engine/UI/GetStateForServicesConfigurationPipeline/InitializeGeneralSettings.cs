using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline
{
    [UsedImplicitly]
    public class InitializeGeneralSettings
    {
        [UsedImplicitly]
        public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
        {
            args.ConfigConstructInfo.AllowResetOption = true;
            args.ConfigConstructInfo.AllowReboot = true;
        }
    }
}
