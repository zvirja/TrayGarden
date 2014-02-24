using System;
using System.Threading;

namespace TrayGarden.Helpers
{
  public interface ISingleInstanceMonitor
  {
    event EventHandler AttemptFromAnotherProcess;

    ManualResetEventSlim EnqueueMonitorDisabling();

    bool TryAcquireOwnershipNotifyIfFail();
  }
}