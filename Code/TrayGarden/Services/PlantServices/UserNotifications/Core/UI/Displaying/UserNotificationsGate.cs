using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;

[UsedImplicitly]
public class UserNotificationsGate : IUserNotificationsGate
{
  public UserNotificationsGate(IDisplayQueueProvider provider)
  {
    Provider = provider;
  }

  protected IDisplayQueueProvider Provider { get; set; }

  public virtual void DiscardAllTasks()
  {
    Provider.DiscardAllTasks();
  }

  public virtual INotificationResultCourier EnqueueToShow(IResultProvider notificationVM, string originator)
  {
    NotificationDisplayTask displayTask = GetDisplayTask(notificationVM, originator);
    if (!AddToDisplayQueue(displayTask))
    {
      displayTask.SetResult(new NotificationResult(ResultCode.Unspecified));
      displayTask.State = NotificationState.Aborted;
    }
    return new NotificationResultCourier(displayTask);
  }

  protected virtual bool AddToDisplayQueue(NotificationDisplayTask task)
  {
    return Provider.EnqueueToDisplay(task);
  }

  protected virtual NotificationDisplayTask GetDisplayTask(IResultProvider notificationVM, string originator)
  {
    return new NotificationDisplayTask(notificationVM, originator);
  }
}