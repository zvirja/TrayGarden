using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Integration
{
  public class UserNotificationsPresenter : ServicePresenterBase<UserNotificationsService>
  {
    public UserNotificationsPresenter()
    {
      this.ServiceName = "User notifications";
      this.ServiceDescription = "If service is enabled, plant is able to display the popup notification windows";
    }

    protected override ServiceForPlantVMBase GetServiceVM(UserNotificationsService serviceInstance, IPlantEx plantEx)
    {
      return new ServiceForPlantWithEnablingPlantBoxBasedVM(
        this.ServiceName,
        this.ServiceDescription,
        serviceInstance.GetPlantLuggage(plantEx));
    }
  }
}