#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public interface INotificationResultCourier
  {
    #region Public Methods and Operators

    bool DiscardIfNotDisplayedYet();

    bool DiscardNotificationInAnyCase();

    NotificationResult GetResultWithWait();

    bool TryGetResultDuringSpecifiedTime(int millisecondsToWait, out NotificationResult result);

    #endregion
  }
}