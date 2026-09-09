using System.Threading;

using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.RestartApp;

public class StopSingleInstanceMonitor(ISingleInstanceMonitor monitor) : IPipelineProcessor<RestartAppArgs>
{
  [UsedImplicitly]
  public virtual void Process(RestartAppArgs args)
  {
    ManualResetEventSlim disablingMonitor = monitor.EnqueueMonitorDisabling();
    disablingMonitor.Wait();
  }
}
