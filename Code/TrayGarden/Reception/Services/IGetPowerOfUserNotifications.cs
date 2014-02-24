using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows plant to create the notification windows, visible to user
  /// </summary>
  public interface IGetPowerOfUserNotifications
  {
    void StoreLordOfNotifications(ILordOfNotifications lordOfNotifications);
  }
}
