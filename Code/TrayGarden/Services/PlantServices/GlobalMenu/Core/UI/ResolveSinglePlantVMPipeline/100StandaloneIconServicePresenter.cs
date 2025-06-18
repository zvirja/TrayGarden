using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

public class StandaloneIconServicePresenter : ServicePresenterBase<StandaloneIconService>
{
  public StandaloneIconServicePresenter()
  {
    this.ServiceName = "Standalone tray icon";
    this.ServiceDescription = "If service is enabled, plant is enabled to show the standalone icon in the system tray.";
  }

  protected override ServiceForPlantVMBase GetServiceVM(StandaloneIconService serviceInstance, IPlantEx plantEx)
  {
    return new ServiceForPlantWithEnablingPlantBoxBasedVM(
      this.ServiceName,
      this.ServiceDescription,
      serviceInstance.GetPlantLuggage(plantEx));
  }
}