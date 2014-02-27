#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.Engine;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Pipelines.Shutdown
{
  public class PlantServicesInformClose
  {
    #region Public Methods and Operators

    public void Process(ShutdownArgs args)
    {
      HatcherGuide<IServicesSteward>.Instance.InformClosingStage();
    }

    #endregion
  }
}