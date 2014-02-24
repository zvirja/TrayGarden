using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public class NotificationDisplayTask
  {
    public NotificationState State { get; set; }
    public ManualResetEvent ResultWaitHandle { get; set; }
    public NotificationResult ObtainedResult { get; set; }
    public IResultProvider RelatedSpecializedNotification { get; set; }
    public Func<NotificationDisplayTask, bool, bool> TaskDiscardHandler { get; set; }
    public string Originator { get; set; }

    public NotificationDisplayTask([NotNull] IResultProvider relatedSpecializedNotification, [NotNull] string originator)
    {
      Assert.ArgumentNotNull(relatedSpecializedNotification, "relatedSpecializedNotification");
      Assert.ArgumentNotNullOrEmpty(originator, "originator");
      RelatedSpecializedNotification = relatedSpecializedNotification;
      Originator = originator;
      State = new NotificationState();
      ResultWaitHandle = new ManualResetEvent(false);
    }

    public virtual void SetResult(NotificationResult result)
    {
      ObtainedResult = result;
      ResultWaitHandle.Set();
    }

    public virtual bool AbortDisplayTask(bool onlyIfNotDisplayedYet)
    {
      bool successfullyDiscarded = DiscardUsingHandler(onlyIfNotDisplayedYet);
      if (!successfullyDiscarded)
        return false;
      if (ObtainedResult == null)
        SetResult(new NotificationResult(ResultCode.Unspecified));
      return true;
    }

    protected virtual bool DiscardUsingHandler(bool onlyIfNotDisplayedYet)
    {
      if (TaskDiscardHandler == null)
        return true;
      var result = TaskDiscardHandler(this, onlyIfNotDisplayedYet);
      return result;
    }
  }
}
