using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  public class NotificationDisplayTask
  {
    public NotificationDisplayTask([NotNull] IResultProvider relatedSpecializedNotification, [NotNull] string originator)
    {
      Assert.ArgumentNotNull(relatedSpecializedNotification, "relatedSpecializedNotification");
      Assert.ArgumentNotNullOrEmpty(originator, "originator");
      this.RelatedSpecializedNotification = relatedSpecializedNotification;
      this.Originator = originator;
      this.State = new NotificationState();
      this.ResultWaitHandle = new ManualResetEvent(false);
    }

    public NotificationResult ObtainedResult { get; set; }

    public string Originator { get; set; }

    public IResultProvider RelatedSpecializedNotification { get; set; }

    public ManualResetEvent ResultWaitHandle { get; set; }

    public NotificationState State { get; set; }

    public Func<NotificationDisplayTask, bool, bool> TaskDiscardHandler { get; set; }

    public virtual bool AbortDisplayTask(bool onlyIfNotDisplayedYet)
    {
      bool successfullyDiscarded = this.DiscardUsingHandler(onlyIfNotDisplayedYet);
      if (!successfullyDiscarded)
      {
        return false;
      }
      if (this.ObtainedResult == null)
      {
        this.SetResult(new NotificationResult(ResultCode.Unspecified));
      }
      return true;
    }

    public virtual void SetResult(NotificationResult result)
    {
      this.ObtainedResult = result;
      this.ResultWaitHandle.Set();
    }

    protected virtual bool DiscardUsingHandler(bool onlyIfNotDisplayedYet)
    {
      if (this.TaskDiscardHandler == null)
      {
        return true;
      }
      var result = this.TaskDiscardHandler(this, onlyIfNotDisplayedYet);
      return result;
    }
  }
}