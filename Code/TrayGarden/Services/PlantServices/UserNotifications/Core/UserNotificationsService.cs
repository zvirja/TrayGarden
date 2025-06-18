using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core;

public class UserNotificationsService : PlantServiceBase<UserNotificationsServicePlantBox>
{
  public UserNotificationsService()
    : base("User notifications", UserNotificationsConfiguration.SettingsBoxName)
  {
    this.ServiceDescription = "This service allows plants to show their custom pop-up notifications.";
  }

  public override void InformClosingStage()
  {
    base.InformClosingStage();
    HatcherGuide<IUserNotificationsGate>.Instance.DiscardAllTasks();
  }

  public override void InitializePlant(TrayGarden.Plants.IPlantEx plantEx)
  {
    base.InitializePlant(plantEx);
    this.InitializePlantInternal(plantEx);
  }

  protected virtual void InitializePlantInternal(IPlantEx plant)
  {
    var workhorse = plant.GetFirstWorkhorseOfType<IGetPowerOfUserNotifications>();
    if (workhorse == null)
    {
      return;
    }
    var plantBox = new UserNotificationsServicePlantBox()
    {
      RelatedPlantEx = plant,
      SettingsBox = plant.MySettingsBox.GetSubBox(this.LuggageName)
    };
    var lord = new LordOfNotifications(plantBox);
    plant.PutLuggage(this.LuggageName, plantBox);
    workhorse.StoreLordOfNotifications(lord);
  }
}