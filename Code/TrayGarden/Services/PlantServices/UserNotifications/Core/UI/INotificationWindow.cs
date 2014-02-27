#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI
{
  [UsedImplicitly]
  public interface INotificationWindow
  {
    #region Public Methods and Operators

    void PrepareAndDisplay(NotificationWindowVM viewModel);

    #endregion
  }
}