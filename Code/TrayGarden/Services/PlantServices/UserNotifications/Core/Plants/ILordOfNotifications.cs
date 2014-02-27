#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Plants
{
  public interface ILordOfNotifications
  {
    #region Public Methods and Operators

    IActionNotification CreateActionNotification(string headerText, string buttonText);

    IInformNotification CreateInformNotification(string textToDisplay);

    IYesNoNotification CreateYesNoNotification(string headerText);

    INotificationResultCourier DisplayNotification(IResultProvider notificationBlank);

    #endregion
  }
}