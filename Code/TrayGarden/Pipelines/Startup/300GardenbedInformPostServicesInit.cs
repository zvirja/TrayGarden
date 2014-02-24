#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Pipelines.Startup
{
  [UsedImplicitly]
  public class GardenbedInformPostServicesInit
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public void Process(StartupArgs args)
    {
      HatcherGuide<IGardenbed>.Instance.InformPostInitStage();
    }

    #endregion
  }
}