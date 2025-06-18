using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Positioning;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying.DisplayProviders
{
  [UsedImplicitly]
  public class TopRightCornerProvider : IDisplayQueueProvider
  {
    protected object Lock = new object();

    public TopRightCornerProvider()
    {
      this.QueuedTasks = new List<DisplayTaskBag>();
      this.DisplayedWaitForResultTasks = new List<DisplayTaskBag>();
    }

    protected List<DisplayTaskBag> DisplayedWaitForResultTasks { get; set; }

    protected TimeSpan NonDisplayedTaskExpiration
    {
      get
      {
        return UserNotificationsConfiguration.ExpirationOfInvisibleNotification.Value;
      }
    }

    protected int NotificationWindowHeight
    {
      get
      {
        return UserNotificationsConfiguration.NotificationWindowHeight.Value;
      }
    }

    protected int NotificationWindowTopIndent
    {
      get
      {
        return UserNotificationsConfiguration.NotificationWindowTopIndent.Value;
      }
    }

    protected int NotificationWindowWidth
    {
      get
      {
        return UserNotificationsConfiguration.NotificationWindowWidth.Value;
      }
    }

    protected List<DisplayTaskBag> QueuedTasks { get; set; }

    protected int SimultaneouslyDisplayedLimit
    {
      get
      {
        return UserNotificationsConfiguration.NotificationWindowMaxDisplayed.Value;
      }
    }

    public virtual void DiscardAllTasks()
    {
      lock (this.Lock)
      {
        DisplayTaskBag[] displayTaskBags = this.QueuedTasks.ToArray();
        foreach (DisplayTaskBag displayTaskBag in displayTaskBags)
        {
          this.DiscardTask(displayTaskBag.Task, false);
        }
        DisplayTaskBag[] taskBags = this.DisplayedWaitForResultTasks.ToArray();
        foreach (DisplayTaskBag displayedWaitForResultTask in taskBags)
        {
          this.DiscardTask(displayedWaitForResultTask.Task, false);
        }
      }
    }

    public virtual bool EnqueueToDisplay(NotificationDisplayTask task)
    {
      var taskBag = this.GetTaskBag(task);
      task.TaskDiscardHandler = this.DiscardTask;
      lock (this.Lock)
      {
        this.AddBagToQueue(taskBag);
        Monitor.Pulse(this.Lock);
        this.ProcessPendingQueue();
      }
      return true;
    }

    protected virtual void AddBagToQueue(DisplayTaskBag task)
    {
      lock (this.Lock)
      {
        this.QueuedTasks.Add(task);
        task.Task.State = NotificationState.InQueue;
      }
    }

    protected virtual DisplayTaskBag DequeueBagAndPrepareItToDisplay()
    {
      lock (this.Lock)
      {
        if (this.DisplayedWaitForResultTasks.Count == this.SimultaneouslyDisplayedLimit)
        {
          return null;
        }
        this.RemoveExpiredTasks();
        if (this.QueuedTasks.Count == 0)
        {
          return null;
        }
        var bag = this.QueuedTasks[0];
        this.QueuedTasks.Remove(bag);
        this.DisplayedWaitForResultTasks.Add(bag);
        bag.Task.State = NotificationState.IsDisplayed;
        return bag;
      }
    }

    protected virtual bool DiscardTask(NotificationDisplayTask task, bool onlyIfNonDisplayed)
    {
      DisplayTaskBag bagToRemove = null;
      try
      {
        lock (this.Lock)
        {
          bagToRemove = this.QueuedTasks.FirstOrDefault(displayTaskBag => displayTaskBag.Task == task);
          if (bagToRemove != null)
          {
            this.QueuedTasks.Remove(bagToRemove);
            return true;
          }
          //Don't try to remove from main collection if flag is true
          if (onlyIfNonDisplayed)
          {
            return false;
          }
          bagToRemove = this.DisplayedWaitForResultTasks.FirstOrDefault(displayTaskBag => displayTaskBag.Task == task);
          if (bagToRemove == null)
          {
            return false;
          }
          this.DisplayedWaitForResultTasks.Remove(bagToRemove);
          return true;
        }
      }
      finally
      {
        if (bagToRemove != null)
        {
          bagToRemove.WindowVM.IsAlive = false;
          bagToRemove.Task.State = NotificationState.Aborted;
        }
      }
    }

    protected virtual PositionSize GetDefaultNotificationPositionSize()
    {
      var width = this.NotificationWindowWidth;
      var height = this.NotificationWindowHeight;
      var top = this.NotificationWindowTopIndent;

      var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
      var left = screenWidth - width;
      return new PositionSize(top, left, width, height);
    }

    protected virtual NotificationWindowVM GetNotificationWindowVM(NotificationDisplayTask task, PositionSize actualPositionAndSize)
    {
      return new NotificationWindowVM(actualPositionAndSize, task.RelatedSpecializedNotification, task.Originator);
    }

    protected virtual DisplayTaskBag GetTaskBag(NotificationDisplayTask task)
    {
      PositionSize positionSize = this.GetDefaultNotificationPositionSize();
      NotificationWindowVM notificationWindowVM = this.GetNotificationWindowVM(task, positionSize);
      return new DisplayTaskBag(task, notificationWindowVM, DateTime.UtcNow, positionSize);
    }

    protected virtual void LastPreparationsAndVisualizeTask(DisplayTaskBag task)
    {
      var uiManager = HatcherGuide<IUIManager>.Instance;
      uiManager.ExecuteActionOnUIThreadAsynchronously(
        delegate
          {
            var window = HatcherGuide<INotificationWindow>.CreateNewInstance();
            task.WindowVM.ResultObtained += this.WindowVM_ResultObtained;
            window.PrepareAndDisplay(task.WindowVM);
          });
    }

    protected virtual void ProcessPendingQueue()
    {
      lock (this.Lock)
      {
        this.RemoveExpiredTasks();
        this.RecalculateDisplayedTasksPositions();
        DisplayTaskBag nextBagToDisplay = this.DequeueBagAndPrepareItToDisplay();
        while (nextBagToDisplay != null)
        {
          this.RecalculateDisplayedTasksPositions();
          this.LastPreparationsAndVisualizeTask(nextBagToDisplay);
          nextBagToDisplay = this.DequeueBagAndPrepareItToDisplay();
        }
      }
    }

    protected virtual void RecalculateDisplayedTasksPositions()
    {
      var currentTop = this.NotificationWindowTopIndent;

      var lockedPositions = new List<int>();
      var nonLockedItems = new List<DisplayTaskBag>();
      //Select locked positions. The item's position is locked if it had a cursor focus at least for one time.
      foreach (DisplayTaskBag displayedWaitForResultTask in this.DisplayedWaitForResultTasks)
      {
        if (displayedWaitForResultTask.WindowVM.IsPositionLocked)
        {
          lockedPositions.Add((int)displayedWaitForResultTask.PositionSize.Top);
        }
        else
        {
          nonLockedItems.Add(displayedWaitForResultTask);
        }
      }
      //we relocate only non locked items.
      foreach (DisplayTaskBag displayTaskBag in nonLockedItems)
      {
        //enumerate to select a free position
        while (lockedPositions.Contains(currentTop))
        {
          currentTop += this.NotificationWindowHeight;
        }
        displayTaskBag.PositionSize.Top = currentTop;
        currentTop += this.NotificationWindowHeight;
      }
    }

    protected virtual void RemoveExpiredTasks()
    {
      DateTime expirationThreshold = DateTime.UtcNow.Subtract(this.NonDisplayedTaskExpiration);
      lock (this.Lock)
      {
        for (int i = this.QueuedTasks.Count - 1; i > -1; i--)
        {
          var bag = this.QueuedTasks[i];
          if (bag.EnqueueTime < expirationThreshold)
          {
            this.QueuedTasks.Remove(bag);
            bag.Task.State = NotificationState.Expired;
            bag.Task.SetResult(new NotificationResult(ResultCode.Unspecified));
          }
        }
      }
    }

    protected virtual void WindowVM_ResultObtained(object sender, ResultObtainedEventArgs e)
    {
      lock (this.Lock)
      {
        var vmFiredEvent = sender as NotificationWindowVM;
        Assert.IsNotNull(vmFiredEvent, "Result source cannot be null");
        var correspondingBag = this.DisplayedWaitForResultTasks.FirstOrDefault(x => x.WindowVM == vmFiredEvent);
        if (correspondingBag == null)
        {
          return;
        }
        this.DisplayedWaitForResultTasks.Remove(correspondingBag);
        correspondingBag.Task.State = NotificationState.Handled;
        correspondingBag.Task.SetResult(vmFiredEvent.Result);
        this.ProcessPendingQueue();
      }
    }
  }
}