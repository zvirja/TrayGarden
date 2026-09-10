using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

public class LordOfNotifications : ILordOfNotifications
{
  private readonly IUserNotificationsGate _userNotificationsGate;

  public LordOfNotifications(IUserNotificationsGate userNotificationsGate, UserNotificationsServicePlantBox relatedPlantBox)
  {
    _userNotificationsGate = userNotificationsGate;
    RelatedPlantBox = relatedPlantBox;
  }

  private UserNotificationsServicePlantBox RelatedPlantBox { get; set; }

  public IActionNotification CreateActionNotification(string headerText, string buttonText)
  {
    return new ActionNotificationVM(headerText, buttonText);
  }

  public IInformNotification CreateInformNotification(string textToDisplay)
  {
    return new InformNotificationVM(textToDisplay);
  }

  public IYesNoNotification CreateYesNoNotification(string headerText)
  {
    return new YesNoNotificationVM(headerText);
  }

  public INotificationResultCourier DisplayNotification(IResultProvider notificationBlank)
  {
    if (!RelatedPlantBox.IsEnabled || !RelatedPlantBox.RelatedPlantEx.IsEnabled)
    {
      return new FakeNotificationResultCourier();
    }
    return _userNotificationsGate.EnqueueToShow(
      notificationBlank,
      RelatedPlantBox.RelatedPlantEx.Plant.HumanSupportingName);
  }
}