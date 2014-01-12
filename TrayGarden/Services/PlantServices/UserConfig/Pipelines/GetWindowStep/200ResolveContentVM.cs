using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep
{
  [UsedImplicitly]
  public class ResolveContentVM
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public void Process(GetUCStepPipelineArgs args)
    {
      args.ConfigurationConstructInfo.ResultControlVM =
        new ConfigurationControlVM(args.ConfigurationConstructInfo.ConfigurationEntries, true);
    }

    #endregion
  }
}