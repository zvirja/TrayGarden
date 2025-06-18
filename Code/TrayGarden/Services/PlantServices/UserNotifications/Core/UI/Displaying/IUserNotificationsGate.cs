using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;

public interface IUserNotificationsGate
{
  void DiscardAllTasks();

  INotificationResultCourier EnqueueToShow(IResultProvider notificationVM, string originator);
}