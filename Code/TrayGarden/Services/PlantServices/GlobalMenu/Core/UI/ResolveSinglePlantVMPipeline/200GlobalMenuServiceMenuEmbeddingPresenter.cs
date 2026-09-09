using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

[UsedImplicitly]
public class GlobalMenuServiceMenuEmbeddingPresenter : ServicePresenterBase<GlobalMenuService>
{
  public GlobalMenuServiceMenuEmbeddingPresenter(IServicesSteward servicesSteward, IUIManager uiManager)
    : base(servicesSteward, uiManager)
  {
    ServiceName = "Embedding to global menu";
    ServiceDescription = "If service is enabled, plant is enabled to embed its row to global menu.";
  }

  protected override ServiceForPlantVMBase GetServiceVM(GlobalMenuService serviceInstance, IPlantEx plantEx)
  {
    var luggage = serviceInstance.GetPlantLuggage(plantEx);
    if (luggage.ToolStripMenuItems == null)
    {
      return null;
    }
    return new ServiceForPlantWithEnablingPlantBoxBasedVM(UIManager, ServiceName, ServiceDescription, luggage);
  }
}
