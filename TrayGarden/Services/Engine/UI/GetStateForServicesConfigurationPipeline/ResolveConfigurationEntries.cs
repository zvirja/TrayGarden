using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Services.Engine.UI.Intergration;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline
{
    [UsedImplicitly]
    public class ResolveConfigurationEntries
    {
        [UsedImplicitly]
        public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
        {
            args.ConfigConstructInfo.ConfigurationEntries = GetConfigurationEntriesFromServices();
        }

        protected virtual List<ConfigurationEntryVMBase> GetConfigurationEntriesFromServices()
        {
            List<IService> services = HatcherGuide<IServicesSteward>.Instance.Services;
            List<ConfigurationEntryVMBase> result = services.Select(ResolveConfigurationEntry).ToList();
            return result;
        }

        protected virtual ConfigurationEntryVMBase ResolveConfigurationEntry(IService service)
        {
            return new ConfigurationEntryForBoolVM(new ConfigurationPlayerServiceAware(service)) { RestoreDefaultValueTooltip = "Reset to actual"};
        }
    }
}
