﻿using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;

[UsedImplicitly]
public class UserNotificationsGate : IUserNotificationsGate
{
  protected bool Initialized { get; set; }

  protected IDisplayQueueProvider Provider { get; set; }

  public virtual void DiscardAllTasks()
  {
    AssertInitialized();
    Provider.DiscardAllTasks();
  }

  public virtual INotificationResultCourier EnqueueToShow(IResultProvider notificationVM, string originator)
  {
    AssertInitialized();
    NotificationDisplayTask displayTask = GetDisplayTask(notificationVM, originator);
    if (!AddToDisplayQueue(displayTask))
    {
      displayTask.SetResult(new NotificationResult(ResultCode.Unspecified));
      displayTask.State = NotificationState.Aborted;
    }
    return new NotificationResultCourier(displayTask);
  }

  [UsedImplicitly]
  public virtual void Initialize([NotNull] IDisplayQueueProvider provider)
  {
    Assert.ArgumentNotNull(provider, "provider");
    Provider = provider;
    Initialized = true;
  }

  protected virtual bool AddToDisplayQueue(NotificationDisplayTask task)
  {
    return Provider.EnqueueToDisplay(task);
  }

  protected virtual void AssertInitialized()
  {
    if (!Initialized)
    {
      throw new NonInitializedException();
    }
  }

  protected virtual NotificationDisplayTask GetDisplayTask(IResultProvider notificationVM, string originator)
  {
    return new NotificationDisplayTask(notificationVM, originator);
  }
}