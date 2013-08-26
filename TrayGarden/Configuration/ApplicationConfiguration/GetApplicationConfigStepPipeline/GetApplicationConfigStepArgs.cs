using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration.Stuff;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
  public class GetApplicationConfigStepArgs : PipelineArgs
  {
    public WindowWithBackStateConstructInfo StepConstructInfo { get; set; }
    public ConfigurationControlConstructInfo ConfigurationConstructInfo { get; set; }

    public GetApplicationConfigStepArgs()
    {
      StepConstructInfo = new WindowWithBackStateConstructInfo();
      ConfigurationConstructInfo = new ConfigurationControlConstructInfo
          {
            ConfigurationEntries = new List<ConfigurationEntryVMBase>()
          };
    }
  }
}
