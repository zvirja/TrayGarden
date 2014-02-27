#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.ForSimplerLife;

#endregion

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
  [UsedImplicitly]
  public class CreateConfigurationVM
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetApplicationConfigStepArgs args)
    {
      ConfigurationControlConstructInfo configurationInfo = args.ConfigurationConstructInfo;
      configurationInfo.BuildControlVM();
      args.StepConstructInfo.ContentVM = configurationInfo.ResultControlVM;
    }

    #endregion
  }
}