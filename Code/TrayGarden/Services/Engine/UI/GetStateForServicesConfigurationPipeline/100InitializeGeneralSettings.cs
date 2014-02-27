#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline
{
  [UsedImplicitly]
  public class InitializeGeneralSettings
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
    {
      args.ConfigConstructInfo.EnableResetAllOption = true;
      args.ConfigConstructInfo.AllowReboot = true;
    }

    #endregion
  }
}