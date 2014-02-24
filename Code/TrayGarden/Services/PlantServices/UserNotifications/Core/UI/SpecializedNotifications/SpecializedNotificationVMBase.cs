using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.UI.Common.VMtoVMapping;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications
{
  public class SpecializedNotificationVMBase : IResultProvider, ISelfViewResolver
  {
    public NotificationResult Result { get; protected set; }
    public event EventHandler<ResultObtainedEventArgs> ResultObtained;

    public virtual Control GetViewToPresentMe()
    {
      return null;
    }

    protected virtual void OnResultObtained(NotificationResult result)
    {
      EventHandler<ResultObtainedEventArgs> handler = ResultObtained;
      if (handler != null) handler(this, new ResultObtainedEventArgs(result));
    }

    protected virtual void SetResultNotifyInterestedMen(NotificationResult result)
    {
      Result = result;
      OnResultObtained(Result);
    }
  }
}
