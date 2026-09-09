using System;
using System.Windows.Controls;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.UI.Common.VMtoVMapping;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications;

public class SpecializedNotificationVMBase : IResultProvider, ISelfViewResolver
{
  public event EventHandler<ResultObtainedEventArgs> ResultObtained;

  public NotificationResult Result { get; private set; }

  public Control GetViewToPresentMe()
  {
    return null;
  }

  private void OnResultObtained(NotificationResult result)
  {
    EventHandler<ResultObtainedEventArgs> handler = ResultObtained;
    if (handler != null)
    {
      handler(this, new ResultObtainedEventArgs(result));
    }
  }

  protected void SetResultNotifyInterestedMen(NotificationResult result)
  {
    Result = result;
    OnResultObtained(Result);
  }
}