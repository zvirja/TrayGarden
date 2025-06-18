using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI
{
  [UsedImplicitly]
  public interface INotificationWindow
  {
    void PrepareAndDisplay(NotificationWindowVM viewModel);
  }
}