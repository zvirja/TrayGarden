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

  private IDisplayQueueProvider Provider { get; set; }

  public void DiscardAllTasks()
  {
    Provider.DiscardAllTasks();
  }

  public INotificationResultCourier EnqueueToShow(IResultProvider notificationVM, string originator)
  {
    NotificationDisplayTask displayTask = GetDisplayTask(notificationVM, originator);
    if (!AddToDisplayQueue(displayTask))
    {
      displayTask.SetResult(new NotificationResult(ResultCode.Unspecified));
      displayTask.State = NotificationState.Aborted;
    }
    return new NotificationResultCourier(displayTask);
  }

  private bool AddToDisplayQueue(NotificationDisplayTask task)
  {
    return Provider.EnqueueToDisplay(task);
  }

  private NotificationDisplayTask GetDisplayTask(IResultProvider notificationVM, string originator)
  {
    return new NotificationDisplayTask(notificationVM, originator);
  }
}