using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public class FakeNotificationResultCourier : INotificationResultCourier
  {
    public virtual bool DiscardIfNotDisplayedYet()
    {
      return false;
    }

    public virtual bool DiscardNotificationInAnyCase()
    {
      return false;
    }

    public virtual NotificationResult GetResultWithWait()
    {
      return new NotificationResult(ResultCode.Unspecified);
    }

    public virtual bool TryGetResultDuringSpecifiedTime(int millisecondsToWait, out NotificationResult result)
    {
      result = this.GetResultWithWait();
      return true;
    }
  }
}