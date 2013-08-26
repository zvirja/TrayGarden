using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration.Stuff;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline
{
  public class UNConfigurationStepArgs : PipelineArgs
  {
    public WindowWithBackStateConstructInfo StateConstructInfo { get; set; }
    public ConfigurationControlConstructInfo ConfigurationConstructInfo { get; set; }

    public UNConfigurationStepArgs()
    {
      StateConstructInfo = new WindowWithBackStateConstructInfo();
      ConfigurationConstructInfo = new ConfigurationControlConstructInfo()
        {
          ConfigurationEntries = new List<ConfigurationEntryVMBase>()
        };
    }
  }
}
