using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.Plants.Intergration
{
  public class AutoLoadAssembliesSetting
  {
    public AutoLoadAssembliesSetting()
    {
      this.SettingDescription =
        "If this setting is enabled, Tray Garden automatically meets with plants in assemblies. The lookup folder is specified in the App.config file.";
    }

    public string SettingDescription { get; set; }

    [UsedImplicitly]
    public virtual void Process(GetApplicationConfigStepArgs args)
    {
      args.ConfigurationConstructInfo.ConfigurationEntries.Add(this.GetConfigurationEntry());
    }

    protected virtual ConfigurationEntryBaseVM GetConfigurationEntry()
    {
      return new BoolConfigurationEntryVM(new AutoLoadPropertyPlayer("Auto load plants", this.SettingDescription));
    }
  }
}