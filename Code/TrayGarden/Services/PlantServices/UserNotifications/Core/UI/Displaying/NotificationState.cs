#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public enum NotificationState
  {
    Unspecified,

    InQueue,

    IsDisplayed,

    Handled,

    Expired,

    Aborted
  }
}