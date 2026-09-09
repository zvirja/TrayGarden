using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core;

public class UserNotificationsService : PlantServiceBase<UserNotificationsServicePlantBox>
{
  private readonly IUserNotificationsGate _userNotificationsGate;

  public UserNotificationsService(IRuntimeSettingsManager runtimeSettingsManager, IUserNotificationsGate userNotificationsGate)
    : base(runtimeSettingsManager, "User notifications", UserNotificationsConfiguration.SettingsBoxName)
  {
    _userNotificationsGate = userNotificationsGate;
    ServiceDescription = "This service allows plants to show their custom pop-up notifications.";
  }

  public override void InformClosingStage()
  {
    base.InformClosingStage();
    _userNotificationsGate.DiscardAllTasks();
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    base.InitializePlant(plantEx);
    InitializePlantInternal(plantEx);
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
      SettingsBox = plant.MySettingsBox.GetSubBox(LuggageName)
    };
    var lord = new LordOfNotifications(_userNotificationsGate, plantBox);
    plant.PutLuggage(LuggageName, plantBox);
    workhorse.StoreLordOfNotifications(lord);
  }
}
