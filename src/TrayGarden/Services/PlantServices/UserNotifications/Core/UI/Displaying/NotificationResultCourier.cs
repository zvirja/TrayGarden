using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;

public class NotificationResultCourier : INotificationResultCourier
{
  public NotificationResultCourier([NotNull] NotificationDisplayTask realTask)
  {
    Assert.ArgumentNotNull(realTask, "realTask");
    RealTask = realTask;
  }

  private NotificationDisplayTask RealTask { get; set; }

  public bool DiscardIfNotDisplayedYet()
  {
    return RealTask.AbortDisplayTask(true);
  }

  public bool DiscardNotificationInAnyCase()
  {
    return RealTask.AbortDisplayTask(false);
  }

  public NotificationResult GetResultWithWait()
  {
    RealTask.ResultWaitHandle.WaitOne();
    NotificationResult obtainedResult = RealTask.ObtainedResult;
    Assert.IsNotNull(obtainedResult, "The obtained result can't be null");
    return obtainedResult;
  }

  public bool TryGetResultDuringSpecifiedTime(int millisecondsToWait, out NotificationResult result)
  {
    result = null;
    if (!RealTask.ResultWaitHandle.WaitOne(millisecondsToWait))
    {
      return false;
    }
    result = RealTask.ObtainedResult;
    return true;
  }
}