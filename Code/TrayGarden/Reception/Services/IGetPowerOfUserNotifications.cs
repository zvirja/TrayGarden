#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

#endregion

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows plant to create the notification windows, visible to user
  /// </summary>
  public interface IGetPowerOfUserNotifications
  {
    #region Public Methods and Operators

    void StoreLordOfNotifications(ILordOfNotifications lordOfNotifications);

    #endregion
  }
}