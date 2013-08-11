using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.UI.WindowWithReturn.Integration
{
  [UsedImplicitly]
  public class ExitOnCloseSetting
  {
    public string SettingDescription { get; set; }

    public ExitOnCloseSetting()
    {
      SettingDescription = "If enabled, exit application if window closed, hide if minimized. Otherwise hide when closed.";
    }

    [UsedImplicitly]
    public virtual void Process(GetApplicationConfigStepArgs args)
    {
      args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetConfigurationEntry());
    }

    protected virtual ConfigurationEntryVMBase GetConfigurationEntry()
    {
      return new ConfigurationEntryForBoolVM(new ExitOnCloseAwarePlayer("Exit on close", SettingDescription));
    }
  }
}
