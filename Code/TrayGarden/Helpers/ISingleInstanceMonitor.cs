#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

#endregion

namespace TrayGarden.Helpers
{
  public interface ISingleInstanceMonitor
  {
    #region Public Events

    event EventHandler AttemptFromAnotherProcess;

    #endregion

    #region Public Methods and Operators

    ManualResetEventSlim EnqueueMonitorDisabling();

    bool TryAcquireOwnershipNotifyIfFail();

    #endregion
  }
}