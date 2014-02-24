#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.ForSimplerLife;

#endregion

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline
{
  public class GetStateForServicesConfigurationPipelineArgs : PipelineArgs
  {
    #region Constructors and Destructors

    public GetStateForServicesConfigurationPipelineArgs()
    {
      this.StateConstructInfo = new WindowWithBackStateConstructInfo();
      this.ConfigConstructInfo = new ConfigurationControlConstructInfo();
    }

    #endregion

    #region Public Properties

    public ConfigurationControlConstructInfo ConfigConstructInfo { get; set; }

    public WindowWithBackStateConstructInfo StateConstructInfo { get; set; }

    #endregion
  }
}