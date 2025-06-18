using System;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

public interface IResultProvider
{
  event EventHandler<ResultObtainedEventArgs> ResultObtained;

  NotificationResult Result { get; }
}