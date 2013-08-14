using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

namespace TrayGarden.Services.PlantServices.UserNotifications.Smorgasbord
{
  public interface IGetPowerOfUserNotifications
  {
    void StoreLordOfNotifications(ILordOfNotifications lordOfNotifications);
  }
}
