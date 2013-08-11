using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Helpers;
using TrayGarden.UI.Configuration;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UI.GetUCStepPipeline
{
    [UsedImplicitly]
    public class ResolveContentVM
    {
        [UsedImplicitly]
        public void Process(GetUCStepPipelineArgs args)
        {
            args.ConfigurationConstructInfo.ResultControlVM = new ConfigurationControlVM(args.ConfigurationConstructInfo.ConfigurationEntries, true);
        }
    }
}
