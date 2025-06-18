using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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