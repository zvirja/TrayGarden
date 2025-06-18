using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;

public class NotificationResultCourier : INotificationResultCourier
{
  public NotificationResultCourier([NotNull] NotificationDisplayTask realTask)
  {
    Assert.ArgumentNotNull(realTask, "realTask");
    this.RealTask = realTask;
  }

  protected NotificationDisplayTask RealTask { get; set; }

  public virtual bool DiscardIfNotDisplayedYet()
  {
    return this.RealTask.AbortDisplayTask(true);
  }

  public virtual bool DiscardNotificationInAnyCase()
  {
    return this.RealTask.AbortDisplayTask(false);
  }

  public virtual NotificationResult GetResultWithWait()
  {
    this.RealTask.ResultWaitHandle.WaitOne();
    NotificationResult obtainedResult = this.RealTask.ObtainedResult;
    Assert.IsNotNull(obtainedResult, "The obtained result can't be null");
    return obtainedResult;
  }

  public virtual bool TryGetResultDuringSpecifiedTime(int millisecondsToWait, out NotificationResult result)
  {
    result = null;
    if (!this.RealTask.ResultWaitHandle.WaitOne(millisecondsToWait))
    {
      return false;
    }
    result = this.RealTask.ObtainedResult;
    return true;
  }
}