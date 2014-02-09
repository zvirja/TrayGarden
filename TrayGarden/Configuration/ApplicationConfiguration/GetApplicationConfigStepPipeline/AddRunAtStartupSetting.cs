#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.Autorun;
using TrayGarden.UI.Configuration.EntryVM;

#endregion

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
  [UsedImplicitly]
  public class AddRunAtStartupSetting
  {
    #region Constructors and Destructors

    public AddRunAtStartupSetting()
    {
      this.Description = "Configures whether start the app at the Windows startup";
    }

    #endregion

    #region Public Properties

    public string Description { get; set; }

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
      var player = new AutorunPlayer("Run at startup", this.Description);
      return new BoolConfigurationEntryVM(player);
    }

    #endregion
  }
}