using System;
using System.Threading;
using System.Threading.Tasks;

using TrayGarden.Diagnostics;

namespace TrayGarden.Helpers;

public class SingleInstanceMonitor : ISingleInstanceMonitor
{
  private static readonly string EventGlobalName = @"Local\TrayGardenEnsureSingleInstance";

  /// <summary>
  /// Indicates whether monitor is disposed. Values:
  /// -1 Not disposed;
  /// 0 - Is disposing. In progress.
  /// 1 - Disposed.
  /// </summary>
  private volatile int disposed;

  private volatile ManualResetEventSlim disposedWaitHandle;

  private volatile EventWaitHandle innerHandle;

  public SingleInstanceMonitor()
  {
    disposed = -1;
    disposedWaitHandle = new ManualResetEventSlim(false);
  }

  public event EventHandler AttemptFromAnotherProcess;

  public ManualResetEventSlim EnqueueMonitorDisabling()
  {
    Assert.IsNotNull(innerHandle, "Monitor should be initialized before");
    if (disposed == -1)
    {
      //this set of lines starts monitor disabling in awaiting loop
      disposed = 0;
      innerHandle.Set();
    }
    return disposedWaitHandle;
  }

  public bool TryAcquireOwnershipNotifyIfFail()
  {
    Assert.IsTrue(innerHandle == null, "Method TryAcquireOwnership() should be called only once.");
    bool createdNewEvent;
    innerHandle = new EventWaitHandle(false, EventResetMode.AutoReset, EventGlobalName, out createdNewEvent);
    if (!createdNewEvent)
    {
      //Notify another process about our attempt
      innerHandle.Set();
      return false;
    }
    Log.For(this).Information("SingleInstanceMonitor: ownership acquired");
    StartAwaitingLoop();
    return true;
  }

  private void ForeignEventAwaitingLoop()
  {
    try
    {
      Assert.IsNotNull(innerHandle, "Inner handle cannot be null at this point");
      while (disposed < 1)
      {
        innerHandle.WaitOne();
        if (disposed == -1)
        {
          Log.For(this).Information("SingleInstanceMonitor: Event from foreign process received");
          NotifyAboutForeignEvent();
        }
        //this.disposed == 0. Disposing in progress.
        //Value cannot be 1, because while() condition cannot allow this.
        else
        {
          Log.For(this).Information("SingleInstanceMonitor: disposing");
          innerHandle.Close();
          disposed = 1;
          disposedWaitHandle.Set();
        }
      }
      Assert.IsTrue(innerHandle.SafeWaitHandle.IsClosed, "Handle should be closed at this point");
    }
    catch (ThreadAbortException)
    {
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Unexpected error in SingleInstanceMonitor awaiting loop");
    }
    finally
    {
      if (!innerHandle.SafeWaitHandle.IsClosed)
      {
        innerHandle.Close();
      }
      disposed = 1;
      disposedWaitHandle.Set();
    }
  }

  private void NotifyAboutForeignEvent()
  {
    Task.Factory.StartNew(OnAttemptFromAnotherProcess);
  }

  private void OnAttemptFromAnotherProcess()
  {
    EventHandler handler = AttemptFromAnotherProcess;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  private void StartAwaitingLoop()
  {
    var checkingThread = new Thread(ForeignEventAwaitingLoop) { IsBackground = true };
    checkingThread.Start();
  }
}