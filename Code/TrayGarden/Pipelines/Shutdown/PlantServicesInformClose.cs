using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Shutdown;

public class PlantServicesInformClose
{
  public void Process(ShutdownArgs args)
  {
    HatcherGuide<IServicesSteward>.Instance.InformClosingStage();
  }
}