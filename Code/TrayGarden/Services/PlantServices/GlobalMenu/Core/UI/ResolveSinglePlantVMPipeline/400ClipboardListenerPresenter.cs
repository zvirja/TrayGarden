using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

[UsedImplicitly]
public class ClipboardListenerPresenter : ServicePresenterBase<ClipboardObserverService>
{
  public ClipboardListenerPresenter()
  {
    ServiceName = "Clipboard listener";
    ServiceDescription = "If service is enabled, plant is enabled to listen clipboard events";
  }

  public static void ViewModel_IsEnabledChanged(ServiceForPlantWithEnablingVM sender, bool newValue)
  {
    var expectedLuggage = sender.Luggage as ClipboardObserverPlantBox;
    Assert.IsNotNull(expectedLuggage, "Luggage is null or wrong type");
    expectedLuggage.IsEnabled = newValue;
  }

  protected override ServiceForPlantVMBase GetServiceVM(ClipboardObserverService serviceInstance, IPlantEx plantEx)
  {
    var vm = new ServiceForPlantWithEnablingVM(ServiceName, ServiceDescription);
    vm.IsEnabledChanged += ViewModel_IsEnabledChanged;
    var plantBox = serviceInstance.GetPlantLuggage(plantEx);
    vm.Luggage = plantBox;
    vm.IsEnabled = plantBox.IsEnabled;
    return vm;
  }
}