using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Positioning;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying.DisplayProviders
{
  public class DisplayTaskBag
  {
    public DateTime EnqueueTime { get; set; }
    public NotificationDisplayTask Task { get; set; }
    public NotificationWindowVM WindowVM { get; set; }
    public PositionSize PositionSize { get; set; }

    public DisplayTaskBag([NotNull] NotificationDisplayTask task, [NotNull] NotificationWindowVM windowVM, DateTime enqueueTime, PositionSize positionSize)
    {
      Assert.ArgumentNotNull(task, "task");
      Assert.ArgumentNotNull(windowVM, "windowVM");
      Task = task;
      WindowVM = windowVM;
      EnqueueTime = enqueueTime;
      PositionSize = positionSize;
    }
  }
}
