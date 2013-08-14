using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Smorgasbord;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core
{
  public class UserNotificationsService : PlantServiceBase<UserNotificationsServicePlantBox>
  {
    public UserNotificationsService()
      : base("User notifications", UserNotificationsConfiguration.SettingsBoxName)
    {
      ServiceDescription = "This service allows plants to show their custom pop-up notifications.";
    }

    public override void InitializePlant(TrayGarden.Plants.IPlantEx plantEx)
    {
      base.InitializePlant(plantEx);
      InitializePlantInternal(plantEx);
    }

    public override void InformClosingStage()
    {
      base.InformClosingStage();
      HatcherGuide<IUserNotificationsGate>.Instance.DiscardAllTasks();
    }

    protected virtual void InitializePlantInternal(IPlantEx plant)
    {
      var workhorse = plant.GetFirstWorkhorseOfType<IGetPowerOfUserNotifications>();
      if (workhorse == null)
        return;
      var plantBox = new UserNotificationsServicePlantBox()
        {
          RelatedPlantEx = plant,
          SettingsBox = plant.MySettingsBox.GetSubBox(LuggageName)
        };
      var lord = new LordOfNotifications(plantBox);
      plant.PutLuggage(LuggageName,plantBox);
      workhorse.StoreLordOfNotifications(lord);
    }
  }
}
