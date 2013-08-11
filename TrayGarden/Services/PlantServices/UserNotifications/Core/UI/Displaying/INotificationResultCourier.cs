using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public interface INotificationResultCourier
  {
    NotificationResult GetResultWithWait();
    bool TryGetResultDuringSpecifiedTime(int millisecondsToWait, out NotificationResult result);
    bool DiscardIfNotDisplayedYet();
    bool DiscardNotificationInAnyCase();
  }
}