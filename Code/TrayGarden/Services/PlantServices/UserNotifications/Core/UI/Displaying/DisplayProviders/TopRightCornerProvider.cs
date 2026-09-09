using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Positioning;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying.DisplayProviders;

[UsedImplicitly]
public class TopRightCornerProvider : IDisplayQueueProvider
{
  private object Lock = new object();

  private readonly IUIManager _uiManager;

  private readonly INotificationWindowFactory _notificationWindowFactory;

  public TopRightCornerProvider(IUIManager uiManager, INotificationWindowFactory notificationWindowFactory)
  {
    _uiManager = uiManager;
    _notificationWindowFactory = notificationWindowFactory;
    QueuedTasks = new List<DisplayTaskBag>();
    DisplayedWaitForResultTasks = new List<DisplayTaskBag>();
  }

  private List<DisplayTaskBag> DisplayedWaitForResultTasks { get; set; }

  private TimeSpan NonDisplayedTaskExpiration
  {
    get
    {
      return UserNotificationsConfiguration.ExpirationOfInvisibleNotification.Value;
    }
  }

  private int NotificationWindowHeight
  {
    get
    {
      return UserNotificationsConfiguration.NotificationWindowHeight.Value;
    }
  }

  private int NotificationWindowTopIndent
  {
    get
    {
      return UserNotificationsConfiguration.NotificationWindowTopIndent.Value;
    }
  }

  private int NotificationWindowWidth
  {
    get
    {
      return UserNotificationsConfiguration.NotificationWindowWidth.Value;
    }
  }

  private List<DisplayTaskBag> QueuedTasks { get; set; }

  private int SimultaneouslyDisplayedLimit
  {
    get
    {
      return UserNotificationsConfiguration.NotificationWindowMaxDisplayed.Value;
    }
  }

  public void DiscardAllTasks()
  {
    lock (Lock)
    {
      DisplayTaskBag[] displayTaskBags = QueuedTasks.ToArray();
      foreach (DisplayTaskBag displayTaskBag in displayTaskBags)
      {
        DiscardTask(displayTaskBag.Task, false);
      }
      DisplayTaskBag[] taskBags = DisplayedWaitForResultTasks.ToArray();
      foreach (DisplayTaskBag displayedWaitForResultTask in taskBags)
      {
        DiscardTask(displayedWaitForResultTask.Task, false);
      }
    }
  }

  public bool EnqueueToDisplay(NotificationDisplayTask task)
  {
    var taskBag = GetTaskBag(task);
    task.TaskDiscardHandler = DiscardTask;
    lock (Lock)
    {
      AddBagToQueue(taskBag);
      Monitor.Pulse(Lock);
      ProcessPendingQueue();
    }
    return true;
  }

  private void AddBagToQueue(DisplayTaskBag task)
  {
    lock (Lock)
    {
      QueuedTasks.Add(task);
      task.Task.State = NotificationState.InQueue;
    }
  }

  private DisplayTaskBag DequeueBagAndPrepareItToDisplay()
  {
    lock (Lock)
    {
      if (DisplayedWaitForResultTasks.Count == SimultaneouslyDisplayedLimit)
      {
        return null;
      }
      RemoveExpiredTasks();
      if (QueuedTasks.Count == 0)
      {
        return null;
      }
      var bag = QueuedTasks[0];
      QueuedTasks.Remove(bag);
      DisplayedWaitForResultTasks.Add(bag);
      bag.Task.State = NotificationState.IsDisplayed;
      return bag;
    }
  }

  private bool DiscardTask(NotificationDisplayTask task, bool onlyIfNonDisplayed)
  {
    DisplayTaskBag bagToRemove = null;
    try
    {
      lock (Lock)
      {
        bagToRemove = QueuedTasks.FirstOrDefault(displayTaskBag => displayTaskBag.Task == task);
        if (bagToRemove != null)
        {
          QueuedTasks.Remove(bagToRemove);
          return true;
        }
        //Don't try to remove from main collection if flag is true
        if (onlyIfNonDisplayed)
        {
          return false;
        }
        bagToRemove = DisplayedWaitForResultTasks.FirstOrDefault(displayTaskBag => displayTaskBag.Task == task);
        if (bagToRemove == null)
        {
          return false;
        }
        DisplayedWaitForResultTasks.Remove(bagToRemove);
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

  private PositionSize GetDefaultNotificationPositionSize()
  {
    var width = NotificationWindowWidth;
    var height = NotificationWindowHeight;
    var top = NotificationWindowTopIndent;

    var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
    var left = screenWidth - width;
    return new PositionSize(top, left, width, height);
  }

  private NotificationWindowVM GetNotificationWindowVM(NotificationDisplayTask task, PositionSize actualPositionAndSize)
  {
    return new NotificationWindowVM(actualPositionAndSize, task.RelatedSpecializedNotification, task.Originator);
  }

  private DisplayTaskBag GetTaskBag(NotificationDisplayTask task)
  {
    PositionSize positionSize = GetDefaultNotificationPositionSize();
    NotificationWindowVM notificationWindowVM = GetNotificationWindowVM(task, positionSize);
    return new DisplayTaskBag(task, notificationWindowVM, DateTime.UtcNow, positionSize);
  }

  private void LastPreparationsAndVisualizeTask(DisplayTaskBag task)
  {
    _uiManager.ExecuteActionOnUIThreadAsynchronously(
      delegate
      {
        var window = _notificationWindowFactory.Create();
        task.WindowVM.ResultObtained += WindowVM_ResultObtained;
        window.PrepareAndDisplay(task.WindowVM);
      });
  }

  private void ProcessPendingQueue()
  {
    lock (Lock)
    {
      RemoveExpiredTasks();
      RecalculateDisplayedTasksPositions();
      DisplayTaskBag nextBagToDisplay = DequeueBagAndPrepareItToDisplay();
      while (nextBagToDisplay != null)
      {
        RecalculateDisplayedTasksPositions();
        LastPreparationsAndVisualizeTask(nextBagToDisplay);
        nextBagToDisplay = DequeueBagAndPrepareItToDisplay();
      }
    }
  }

  private void RecalculateDisplayedTasksPositions()
  {
    var currentTop = NotificationWindowTopIndent;

    var lockedPositions = new List<int>();
    var nonLockedItems = new List<DisplayTaskBag>();
    //Select locked positions. The item's position is locked if it had a cursor focus at least for one time.
    foreach (DisplayTaskBag displayedWaitForResultTask in DisplayedWaitForResultTasks)
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
        currentTop += NotificationWindowHeight;
      }
      displayTaskBag.PositionSize.Top = currentTop;
      currentTop += NotificationWindowHeight;
    }
  }

  private void RemoveExpiredTasks()
  {
    DateTime expirationThreshold = DateTime.UtcNow.Subtract(NonDisplayedTaskExpiration);
    lock (Lock)
    {
      for (int i = QueuedTasks.Count - 1; i > -1; i--)
      {
        var bag = QueuedTasks[i];
        if (bag.EnqueueTime < expirationThreshold)
        {
          QueuedTasks.Remove(bag);
          bag.Task.State = NotificationState.Expired;
          bag.Task.SetResult(new NotificationResult(ResultCode.Unspecified));
        }
      }
    }
  }

  private void WindowVM_ResultObtained(object sender, ResultObtainedEventArgs e)
  {
    lock (Lock)
    {
      var vmFiredEvent = sender as NotificationWindowVM;
      Assert.IsNotNull(vmFiredEvent, "Result source cannot be null");
      var correspondingBag = DisplayedWaitForResultTasks.FirstOrDefault(x => x.WindowVM == vmFiredEvent);
      if (correspondingBag == null)
      {
        return;
      }
      DisplayedWaitForResultTasks.Remove(correspondingBag);
      correspondingBag.Task.State = NotificationState.Handled;
      correspondingBag.Task.SetResult(vmFiredEvent.Result);
      ProcessPendingQueue();
    }
  }
}