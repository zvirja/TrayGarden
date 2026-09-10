using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;

public class FakeNotificationResultCourier : INotificationResultCourier
{
  public bool DiscardIfNotDisplayedYet()
  {
    return false;
  }

  public bool DiscardNotificationInAnyCase()
  {
    return false;
  }

  public NotificationResult GetResultWithWait()
  {
    return new NotificationResult(ResultCode.Unspecified);
  }

  public bool TryGetResultDuringSpecifiedTime(int millisecondsToWait, out NotificationResult result)
  {
    result = GetResultWithWait();
    return true;
  }
}