using System;
using System.Threading;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.RestartApp;

public class StopSingleInstanceMonitor(ISingleInstanceMonitor monitor) : IPipelineProcessor<RestartAppArgs>
{
  [UsedImplicitly]
  public void Process(RestartAppArgs args)
  {
    Log.For(this).Information("StopSingleInstanceMonitor: requesting monitor shutdown");
    ManualResetEventSlim disablingMonitor = monitor.EnqueueMonitorDisabling();
    bool released = disablingMonitor.Wait(TimeSpan.FromSeconds(5));
    Log.For(this).Information("StopSingleInstanceMonitor: monitor released within timeout: {Released}", released);
  }
}
