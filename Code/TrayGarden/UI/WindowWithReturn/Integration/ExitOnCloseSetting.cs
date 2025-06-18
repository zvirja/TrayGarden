using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.UI.WindowWithReturn.Integration;

[UsedImplicitly]
public class ExitOnCloseSetting
{
  public ExitOnCloseSetting()
  {
    this.SettingDescription = "If enabled, exit application if window closed, hide if minimized. Otherwise hide when closed.";
  }

  public string SettingDescription { get; set; }

  [UsedImplicitly]
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
    args.ConfigurationConstructInfo.ConfigurationEntries.Add(this.GetConfigurationEntry());
  }

  protected virtual ConfigurationEntryBaseVM GetConfigurationEntry()
  {
    return new BoolConfigurationEntryVM(new ExitOnClosePlayer("Exit on close", this.SettingDescription));
  }
}