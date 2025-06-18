using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

public interface IResultProvider
{
  event EventHandler<ResultObtainedEventArgs> ResultObtained;

  NotificationResult Result { get; }
}