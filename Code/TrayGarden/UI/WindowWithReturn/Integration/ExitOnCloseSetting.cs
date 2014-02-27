#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.UI.Configuration.EntryVM;

#endregion

namespace TrayGarden.UI.WindowWithReturn.Integration
{
  [UsedImplicitly]
  public class ExitOnCloseSetting
  {
    #region Constructors and Destructors

    public ExitOnCloseSetting()
    {
      this.SettingDescription = "If enabled, exit application if window closed, hide if minimized. Otherwise hide when closed.";
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
      return new BoolConfigurationEntryVM(new ExitOnClosePlayer("Exit on close", this.SettingDescription));
    }

    #endregion
  }
}