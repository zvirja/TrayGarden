using JetBrains.Annotations;

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
    this.AssertInitialized();
    this.Provider.DiscardAllTasks();
  }

  public virtual INotificationResultCourier EnqueueToShow(IResultProvider notificationVM, string originator)
  {
    this.AssertInitialized();
    NotificationDisplayTask displayTask = this.GetDisplayTask(notificationVM, originator);
    if (!this.AddToDisplayQueue(displayTask))
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
    this.Provider = provider;
    this.Initialized = true;
  }

  protected virtual bool AddToDisplayQueue(NotificationDisplayTask task)
  {
    return this.Provider.EnqueueToDisplay(task);
  }

  protected virtual void AssertInitialized()
  {
    if (!this.Initialized)
    {
      throw new NonInitializedException();
    }
  }

  protected virtual NotificationDisplayTask GetDisplayTask(IResultProvider notificationVM, string originator)
  {
    return new NotificationDisplayTask(notificationVM, originator);
  }
}