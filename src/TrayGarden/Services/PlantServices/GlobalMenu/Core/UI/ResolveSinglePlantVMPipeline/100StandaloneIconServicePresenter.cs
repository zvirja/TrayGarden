using TrayGarden.Plants;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

public class StandaloneIconServicePresenter : ServicePresenterBase<StandaloneIconService>
{
  public StandaloneIconServicePresenter(IServicesSteward servicesSteward, IUIManager uiManager)
    : base(servicesSteward, uiManager)
  {
    ServiceName = "Standalone tray icon";
    ServiceDescription = "If service is enabled, plant is enabled to show the standalone icon in the system tray.";
  }

  protected override ServiceForPlantVMBase GetServiceVM(StandaloneIconService serviceInstance, IPlantEx plantEx)
  {
    return new ServiceForPlantWithEnablingPlantBoxBasedVM(
      UIManager,
      ServiceName,
      ServiceDescription,
      serviceInstance.GetPlantLuggage(plantEx));
  }
}
