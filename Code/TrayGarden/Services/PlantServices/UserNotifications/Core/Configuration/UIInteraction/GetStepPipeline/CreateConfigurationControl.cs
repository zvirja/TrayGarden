using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline
{
  [UsedImplicitly]
  public class CreateConfigurationControl
  {
    [UsedImplicitly]
    public virtual void Process(UNConfigurationStepArgs args)
    {
      args.ConfigurationConstructInfo.BuildControlVM();
    }
  }
}