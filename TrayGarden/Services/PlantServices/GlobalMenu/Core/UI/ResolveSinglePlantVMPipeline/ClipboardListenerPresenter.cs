using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
    [UsedImplicitly]
    public class ClipboardListenerPresenter : ServicePresenterBase<ClipboardObserverService>
    {
        public ClipboardListenerPresenter()
        {
            ServiceName = "Clipboard listener";
            ServiceDescription = "If service is enabled, plant is enabled to listen clipboard events";
        }

        protected override ServiceForPlantVMBase GetServiceVM(ClipboardObserverService serviceInstance, IPlantEx plantEx)
        {
            var vm = new ServiceForPlantWithEnablingVM(ServiceName, ServiceDescription);
            vm.IsEnabledChanged += ViewModel_IsEnabledChanged;
            vm.Luggage = serviceInstance.GetPlantLuggage(plantEx);
            return vm;
        }

        public static void ViewModel_IsEnabledChanged(ServiceForPlantWithEnablingVM sender, bool newValue)
        {
            throw new NotImplementedException();
        }
    }
}
