using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

public interface ILordOfNotifications
{
  IActionNotification CreateActionNotification(string headerText, string buttonText);

  IInformNotification CreateInformNotification(string textToDisplay);

  IYesNoNotification CreateYesNoNotification(string headerText);

  INotificationResultCourier DisplayNotification(IResultProvider notificationBlank);
}