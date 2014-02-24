using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public class NotificationResultCourier : INotificationResultCourier
  {
    protected NotificationDisplayTask RealTask { get; set; }

    public NotificationResultCourier([NotNull] NotificationDisplayTask realTask)
    {
      Assert.ArgumentNotNull(realTask, "realTask");
      RealTask = realTask;
    }

    public virtual NotificationResult GetResultWithWait()
    {
      RealTask.ResultWaitHandle.WaitOne();
      NotificationResult obtainedResult = RealTask.ObtainedResult;
      Assert.IsNotNull(obtainedResult,"The obtained result can't be null");
      return obtainedResult;
    }

    public virtual bool TryGetResultDuringSpecifiedTime(int millisecondsToWait, out NotificationResult result)
    {
      result = null;
      if (!RealTask.ResultWaitHandle.WaitOne(millisecondsToWait))
        return false;
      result = RealTask.ObtainedResult;
      return true;
    }

    public virtual bool DiscardIfNotDisplayedYet()
    {
      return RealTask.AbortDisplayTask(true);
    }

    public virtual bool DiscardNotificationInAnyCase()
    {
      return RealTask.AbortDisplayTask(false);
    }
  }
}
