using System;
using System.Threading;
using System.Threading.Tasks;

using TrayGarden.Diagnostics;

namespace TrayGarden.Helpers;

public class SingleInstanceMonitor : ISingleInstanceMonitor
{
  protected static readonly string EventGlobalName = @"Local\TrayGardenEnsureSingleInstance";

  /// <summary>
  /// Indicates whether monitor is disposed. Values:
  /// -1 Not disposed;
  /// 0 - Is disposing. In progress.
  /// 1 - Disposed.
  /// </summary>
  protected volatile int disposed;

  protected volatile ManualResetEventSlim disposedWaitHandle;

  protected volatile EventWaitHandle innerHandle;

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

  public virtual bool TryAcquireOwnershipNotifyIfFail()
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
    Log.Info("SingleInstanceMonitor: ownership acquired", this);
    StartAwaitingLoop();
    return true;
  }

  protected virtual void ForeignEventAwaitingLoop()
  {
    try
    {
      Assert.IsNotNull(innerHandle, "Inner handle cannot be null at this point");
      while (disposed < 1)
      {
        innerHandle.WaitOne();
        if (disposed == -1)
        {
          Log.Info("SingleInstanceMonitor: Event from foreign process received", this);
          NotifyAboutForeignEvent();
        }
        //this.disposed == 0. Disposing in progress.
        //Value cannot be 1, because while() condition cannot allow this.
        else
        {
          Log.Info("SingleInstanceMonitor: disposing", this);
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
      Log.Error("Unexpected error in SingleInstanceMonitor awaiting loop", ex, this);
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

  protected virtual void NotifyAboutForeignEvent()
  {
    Task.Factory.StartNew(OnAttemptFromAnotherProcess);
  }

  protected virtual void OnAttemptFromAnotherProcess()
  {
    EventHandler handler = AttemptFromAnotherProcess;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  protected virtual void StartAwaitingLoop()
  {
    var checkingThread = new Thread(ForeignEventAwaitingLoop) { IsBackground = true };
    checkingThread.Start();
  }
}