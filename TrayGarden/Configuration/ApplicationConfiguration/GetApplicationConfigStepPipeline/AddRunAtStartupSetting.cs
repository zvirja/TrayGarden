using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration.ApplicationConfiguration.Autorun;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
    [UsedImplicitly]
    public class AddRunAtStartupSetting
    {
        public string Description { get; set; }

        public AddRunAtStartupSetting()
        {
            Description = "Configures whether start the app at the Windows startup";
        }

        [UsedImplicitly]
        public virtual void Process(GetApplicationConfigStepArgs args)
        {
            args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetConfigurationEntry());
        }

        protected virtual ConfigurationEntryVMBase GetConfigurationEntry()
        {
            var player = new AutorunAwarePlayer("Run at startup", Description);
            return new ConfigurationEntryForBoolVM(player);
        }
    }
}
