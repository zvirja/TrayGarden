using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.RestartApp;

public class StopSingleInstanceMonitor
{
  [UsedImplicitly]
  public virtual void Process(RestartAppArgs args)
  {
    ManualResetEventSlim monitor = HatcherGuide<ISingleInstanceMonitor>.Instance.EnqueueMonitorDisabling();
    monitor.Wait();
  }
}