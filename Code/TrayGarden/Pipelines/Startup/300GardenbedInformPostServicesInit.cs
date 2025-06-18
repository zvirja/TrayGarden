using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Startup;

[UsedImplicitly]
public class GardenbedInformPostServicesInit
{
  [UsedImplicitly]
  public void Process(StartupArgs args)
  {
    HatcherGuide<IGardenbed>.Instance.InformPostInitStage();
  }
}