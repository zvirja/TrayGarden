#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
  [UsedImplicitly]
  public class ClipboardListenerPresenter : ServicePresenterBase<ClipboardObserverService>
  {
    #region Constructors and Destructors

    public ClipboardListenerPresenter()
    {
      this.ServiceName = "Clipboard listener";
      this.ServiceDescription = "If service is enabled, plant is enabled to listen clipboard events";
    }

    #endregion

    #region Public Methods and Operators

    public static void ViewModel_IsEnabledChanged(ServiceForPlantWithEnablingVM sender, bool newValue)
    {
      var expectedLuggage = sender.Luggage as ClipboardObserverPlantBox;
      Assert.IsNotNull(expectedLuggage, "Luggage is null or wrong type");
      expectedLuggage.IsEnabled = newValue;
    }

    #endregion

    #region Methods

    protected override ServiceForPlantVMBase GetServiceVM(ClipboardObserverService serviceInstance, IPlantEx plantEx)
    {
      var vm = new ServiceForPlantWithEnablingVM(this.ServiceName, this.ServiceDescription);
      vm.IsEnabledChanged += ViewModel_IsEnabledChanged;
      var plantBox = serviceInstance.GetPlantLuggage(plantEx);
      vm.Luggage = plantBox;
      vm.IsEnabled = plantBox.IsEnabled;
      return vm;
    }

    #endregion
  }
}