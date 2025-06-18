using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

public class ResultObtainedEventArgs : EventArgs
{
  public ResultObtainedEventArgs(NotificationResult result)
  {
    this.Result = result;
  }

  public NotificationResult Result { get; set; }
}