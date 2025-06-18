﻿using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Integration;

public class UserNotificationsPresenter : ServicePresenterBase<UserNotificationsService>
{
  public UserNotificationsPresenter()
  {
    ServiceName = "User notifications";
    ServiceDescription = "If service is enabled, plant is able to display the popup notification windows";
  }

  protected override ServiceForPlantVMBase GetServiceVM(UserNotificationsService serviceInstance, IPlantEx plantEx)
  {
    return new ServiceForPlantWithEnablingPlantBoxBasedVM(
      ServiceName,
      ServiceDescription,
      serviceInstance.GetPlantLuggage(plantEx));
  }
}