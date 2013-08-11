using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering
{
  public interface IResultProvider
  {
    NotificationResult Result { get; }
    event EventHandler<ResultObtainedEventArgs> ResultObtained;
  }
}
