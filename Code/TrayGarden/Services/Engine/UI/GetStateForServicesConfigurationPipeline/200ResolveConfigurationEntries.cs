using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using TrayGarden.Services.Engine.UI.Intergration;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

public class ResolveConfigurationEntries
{
  [UsedImplicitly]
  public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
  {
    args.ConfigConstructInfo.ConfigurationEntries = GetConfigurationEntriesFromServices();
  }

  protected virtual List<ConfigurationEntryBaseVM> GetConfigurationEntriesFromServices()
  {
    List<IService> services = HatcherGuide<IServicesSteward>.Instance.Services;
    List<ConfigurationEntryBaseVM> result = services.Select(ResolveConfigurationEntry).ToList();
    return result;
  }

  protected virtual ConfigurationEntryBaseVM ResolveConfigurationEntry(IService service)
  {
    return new BoolConfigurationEntryVM(new ConfigurationPlayerService(service)) { RestoreDefaultValueTooltip = "Reset to actual" };
  }
}