using System;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

public class ResultObtainedEventArgs : EventArgs
{
  public ResultObtainedEventArgs(NotificationResult result)
  {
    Result = result;
  }

  public NotificationResult Result { get; set; }
}