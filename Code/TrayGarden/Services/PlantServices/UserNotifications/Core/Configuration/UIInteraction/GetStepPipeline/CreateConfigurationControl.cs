#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline
{
  [UsedImplicitly]
  public class CreateConfigurationControl
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(UNConfigurationStepArgs args)
    {
      args.ConfigurationConstructInfo.BuildControlVM();
    }

    #endregion
  }
}