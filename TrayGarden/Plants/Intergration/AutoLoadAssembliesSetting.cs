using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Plants.Intergration
{
    public class AutoLoadAssembliesSetting
    {
        public string SettingDescription { get; set; }

        public AutoLoadAssembliesSetting()
        {
            SettingDescription = "If this setting is enabled, Tray Garden automatically meets with plants in assemblies. The lookup folder is specified in the App.config file.";
        }

        public virtual void Process(GetApplicationConfigStepArgs args)
        {
            args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetConfigurationEntry());
        }

        protected virtual ConfigurationEntryVMBase GetConfigurationEntry()
        {
            return
                new ConfigurationEntryForBoolVM(new AutoLoadPropertyAwarePlayer("Auto load plants", SettingDescription));
        }
    }
}
