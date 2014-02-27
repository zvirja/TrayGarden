#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public interface IDisplayQueueProvider
  {
    #region Public Methods and Operators

    void DiscardAllTasks();

    bool EnqueueToDisplay(NotificationDisplayTask task);

    #endregion
  }
}