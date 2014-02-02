#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Pipelines.RestartApp
{
  public class StopSingleInstanceMonitor
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(RestartAppArgs args)
    {
      ManualResetEventSlim monitor = HatcherGuide<ISingleInstanceMonitor>.Instance.EnqueueMonitorDisabling();
      monitor.Wait();
    }

    #endregion
  }
}