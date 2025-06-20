﻿using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

[UsedImplicitly]
public class GlobalMenuServiceIconChangePresenter : ServicePresenterBase<GlobalMenuService>
{
  public GlobalMenuServiceIconChangePresenter()
  {
    ServiceName = "Changing of global icon";
    ServiceDescription = "If service is enabled, plant is enabled to change the global tray icon.";
  }

  protected static void ViewModel_IsEnabledChanged(ServiceForPlantWithEnablingVM sender, bool newValue)
  {
    var expectedLuggage = sender.Luggage as GlobalMenuPlantBox;
    Assert.IsNotNull(expectedLuggage, "Luggage is null or wrong type");
    expectedLuggage.GlobalNotifyIconChangerEnabled = newValue;
  }

  protected override ServiceForPlantVMBase GetServiceVM(GlobalMenuService serviceInstance, IPlantEx plantEx)
  {
    var vm = new ServiceForPlantWithEnablingVM(ServiceName, ServiceDescription);
    var plantBox = serviceInstance.GetPlantLuggage(plantEx);
    if (plantBox.GlobalNotifyIconChanger == null)
    {
      return null;
    }
    vm.IsEnabled = plantBox.GlobalNotifyIconChangerEnabled;
    vm.Luggage = plantBox;
    vm.IsEnabledChanged += ViewModel_IsEnabledChanged;
    return vm;
  }
}