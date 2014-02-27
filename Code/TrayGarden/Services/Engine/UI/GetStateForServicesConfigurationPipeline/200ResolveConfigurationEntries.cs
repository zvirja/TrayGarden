#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Services.Engine.UI.Intergration;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Configuration.EntryVM;

#endregion

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline
{
  public class ResolveConfigurationEntries
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
    {
      args.ConfigConstructInfo.ConfigurationEntries = this.GetConfigurationEntriesFromServices();
    }

    #endregion

    #region Methods

    protected virtual List<ConfigurationEntryBaseVM> GetConfigurationEntriesFromServices()
    {
      List<IService> services = HatcherGuide<IServicesSteward>.Instance.Services;
      List<ConfigurationEntryBaseVM> result = services.Select(this.ResolveConfigurationEntry).ToList();
      return result;
    }

    protected virtual ConfigurationEntryBaseVM ResolveConfigurationEntry(IService service)
    {
      return new BoolConfigurationEntryVM(new ConfigurationPlayerService(service)) { RestoreDefaultValueTooltip = "Reset to actual" };
    }

    #endregion
  }
}