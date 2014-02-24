#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.UI.Configuration.EntryVM;

#endregion

namespace TrayGarden.Plants.Intergration
{
  public class AutoLoadAssembliesSetting
  {
    #region Constructors and Destructors

    public AutoLoadAssembliesSetting()
    {
      this.SettingDescription =
        "If this setting is enabled, Tray Garden automatically meets with plants in assemblies. The lookup folder is specified in the App.config file.";
    }

    #endregion

    #region Public Properties

    public string SettingDescription { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetApplicationConfigStepArgs args)
    {
      args.ConfigurationConstructInfo.ConfigurationEntries.Add(this.GetConfigurationEntry());
    }

    #endregion

    #region Methods

    protected virtual ConfigurationEntryBaseVM GetConfigurationEntry()
    {
      return new BoolConfigurationEntryVM(new AutoLoadPropertyPlayer("Auto load plants", this.SettingDescription));
    }

    #endregion
  }
}