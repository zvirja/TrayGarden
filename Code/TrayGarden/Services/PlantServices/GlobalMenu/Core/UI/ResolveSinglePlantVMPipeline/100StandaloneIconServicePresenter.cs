#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
  public class StandaloneIconServicePresenter : ServicePresenterBase<StandaloneIconService>
  {
    #region Constructors and Destructors

    public StandaloneIconServicePresenter()
    {
      this.ServiceName = "Standalone tray icon";
      this.ServiceDescription = "If service is enabled, plant is enabled to show the standalone icon in the system tray.";
    }

    #endregion

    #region Methods

    protected override ServiceForPlantVMBase GetServiceVM(StandaloneIconService serviceInstance, IPlantEx plantEx)
    {
      return new ServiceForPlantWithEnablingPlantBoxBasedVM(
        this.ServiceName,
        this.ServiceDescription,
        serviceInstance.GetPlantLuggage(plantEx));
    }

    #endregion
  }
}