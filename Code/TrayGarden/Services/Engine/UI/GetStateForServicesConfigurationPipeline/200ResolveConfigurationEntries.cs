using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.Engine.UI.Intergration;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

public class ResolveConfigurationEntries(IServicesSteward servicesSteward)
  : IPipelineProcessor<GetStateForServicesConfigurationPipelineArgs>
{
  [UsedImplicitly]
  public void Process(GetStateForServicesConfigurationPipelineArgs args)
  {
    args.ConfigConstructInfo.ConfigurationEntries = GetConfigurationEntriesFromServices();
  }

  private List<ConfigurationEntryBaseVM> GetConfigurationEntriesFromServices()
  {
    List<IService> services = servicesSteward.Services;
    List<ConfigurationEntryBaseVM> result = services.Select(ResolveConfigurationEntry).ToList();
    return result;
  }

  private ConfigurationEntryBaseVM ResolveConfigurationEntry(IService service)
  {
    return new BoolConfigurationEntryVM(new ConfigurationPlayerService(service)) { RestoreDefaultValueTooltip = "Reset to actual" };
  }
}
