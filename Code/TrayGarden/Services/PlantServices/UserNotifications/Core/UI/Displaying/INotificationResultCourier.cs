using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;

public interface INotificationResultCourier
{
  bool DiscardIfNotDisplayedYet();

  bool DiscardNotificationInAnyCase();

  NotificationResult GetResultWithWait();

  bool TryGetResultDuringSpecifiedTime(int millisecondsToWait, out NotificationResult result);
}