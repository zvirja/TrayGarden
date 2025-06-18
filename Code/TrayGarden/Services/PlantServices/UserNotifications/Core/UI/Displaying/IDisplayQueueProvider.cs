using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public interface IDisplayQueueProvider
  {
    void DiscardAllTasks();

    bool EnqueueToDisplay(NotificationDisplayTask task);
  }
}