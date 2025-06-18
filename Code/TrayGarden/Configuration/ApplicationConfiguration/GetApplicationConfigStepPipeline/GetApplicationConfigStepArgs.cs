using System.Collections.Generic;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration.EntryVM;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;

public class GetApplicationConfigStepArgs : PipelineArgs
{
  public GetApplicationConfigStepArgs()
  {
    StepConstructInfo = new WindowWithBackStateConstructInfo();
    ConfigurationConstructInfo = new ConfigurationControlConstructInfo
    {
      ConfigurationEntries = new List<ConfigurationEntryBaseVM>()
    };
  }

  public ConfigurationControlConstructInfo ConfigurationConstructInfo { get; set; }

  public WindowWithBackStateConstructInfo StepConstructInfo { get; set; }
}