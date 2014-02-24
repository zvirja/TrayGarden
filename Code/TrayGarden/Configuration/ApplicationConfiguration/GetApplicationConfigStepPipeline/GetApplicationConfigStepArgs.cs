#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration.EntryVM;
using TrayGarden.UI.ForSimplerLife;

#endregion

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
  public class GetApplicationConfigStepArgs : PipelineArgs
  {
    #region Constructors and Destructors

    public GetApplicationConfigStepArgs()
    {
      this.StepConstructInfo = new WindowWithBackStateConstructInfo();
      this.ConfigurationConstructInfo = new ConfigurationControlConstructInfo
                                          {
                                            ConfigurationEntries = new List<ConfigurationEntryBaseVM>()
                                          };
    }

    #endregion

    #region Public Properties

    public ConfigurationControlConstructInfo ConfigurationConstructInfo { get; set; }

    public WindowWithBackStateConstructInfo StepConstructInfo { get; set; }

    #endregion
  }
}