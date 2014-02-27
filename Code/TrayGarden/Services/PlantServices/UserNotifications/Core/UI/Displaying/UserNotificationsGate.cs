#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying
{
  [UsedImplicitly]
  public class UserNotificationsGate : IUserNotificationsGate
  {
    #region Properties

    protected bool Initialized { get; set; }

    protected IDisplayQueueProvider Provider { get; set; }

    #endregion

    #region Public Methods and Operators

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

    #endregion

    #region Methods

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

    #endregion
  }
}