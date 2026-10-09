using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core;

public class UserNotificationsService : PlantServiceBase<UserNotificationsServicePlantBox>
{
  private readonly IToastGate _toastGate;

  public UserNotificationsService(IRuntimeSettingsManager runtimeSettingsManager, IToastGate toastGate)
    : base(runtimeSettingsManager, "User notifications", "UserNotificationsService")
  {
    _toastGate = toastGate;
    ServiceDescription = "This service allows plants to show their Windows toast notifications.";
  }

  public override void InformClosingStage()
  {
    base.InformClosingStage();
    _toastGate.DiscardAll();
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    base.InitializePlant(plantEx);
    InitializePlantInternal(plantEx);
  }

  private void InitializePlantInternal(IPlantEx plant)
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
    var notifier = new PlantNotifier(_toastGate, plantBox);
    plant.PutLuggage(LuggageName, plantBox);
    workhorse.StoreNotifier(notifier);
  }
}
