﻿using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

[UsedImplicitly]
public class GlobalMenuServiceMenuEmbeddingPresenter : ServicePresenterBase<GlobalMenuService>
{
  public GlobalMenuServiceMenuEmbeddingPresenter()
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
    return new ServiceForPlantWithEnablingPlantBoxBasedVM(ServiceName, ServiceDescription, luggage);
  }
}