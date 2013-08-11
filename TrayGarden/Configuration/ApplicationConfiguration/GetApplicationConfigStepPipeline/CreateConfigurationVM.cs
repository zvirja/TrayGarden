using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
    [UsedImplicitly]
    public class CreateConfigurationVM
    {
        [UsedImplicitly]
        public virtual void Process(GetApplicationConfigStepArgs args)
        {
            ConfigurationControlConstructInfo configurationInfo = args.ConfigurationConstructInfo;
            configurationInfo.ResultControlVM = new ConfigurationControlVM(configurationInfo.ConfigurationEntries,
                                                                           configurationInfo.AllowResetOption)
                {
                    CalculateRebootOption = configurationInfo.AllowReboot,
                    ConfigurationDescription = configurationInfo.ConfigurationDescription
                };
            args.StepConstructInfo.ContentVM = configurationInfo.ResultControlVM;
        }
    }
}
