#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Positioning;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying.DisplayProviders
{
  public class DisplayTaskBag
  {
    #region Constructors and Destructors

    public DisplayTaskBag(
      [NotNull] NotificationDisplayTask task,
      [NotNull] NotificationWindowVM windowVM,
      DateTime enqueueTime,
      PositionSize positionSize)
    {
      Assert.ArgumentNotNull(task, "task");
      Assert.ArgumentNotNull(windowVM, "windowVM");
      this.Task = task;
      this.WindowVM = windowVM;
      this.EnqueueTime = enqueueTime;
      this.PositionSize = positionSize;
    }

    #endregion

    #region Public Properties

    public DateTime EnqueueTime { get; set; }

    public PositionSize PositionSize { get; set; }

    public NotificationDisplayTask Task { get; set; }

    public NotificationWindowVM WindowVM { get; set; }

    #endregion
  }
}