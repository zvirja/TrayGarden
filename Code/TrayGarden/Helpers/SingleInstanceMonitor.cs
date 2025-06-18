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
    this.disposed = -1;
    this.disposedWaitHandle = new ManualResetEventSlim(false);
  }

  public event EventHandler AttemptFromAnotherProcess;

  public ManualResetEventSlim EnqueueMonitorDisabling()
  {
    Assert.IsNotNull(this.innerHandle, "Monitor should be initialized before");
    if (this.disposed == -1)
    {
      //this set of lines starts monitor disabling in awaiting loop
      this.disposed = 0;
      this.innerHandle.Set();
    }
    return this.disposedWaitHandle;
  }

  public virtual bool TryAcquireOwnershipNotifyIfFail()
  {
    Assert.IsTrue(this.innerHandle == null, "Method TryAcquireOwnership() should be called only once.");
    bool createdNewEvent;
    this.innerHandle = new EventWaitHandle(false, EventResetMode.AutoReset, EventGlobalName, out createdNewEvent);
    if (!createdNewEvent)
    {
      //Notify another process about our attempt
      this.innerHandle.Set();
      return false;
    }
    Log.Info("SingleInstanceMonitor: ownership acquired", this);
    this.StartAwaitingLoop();
    return true;
  }

  protected virtual void ForeignEventAwaitingLoop()
  {
    try
    {
      Assert.IsNotNull(this.innerHandle, "Inner handle cannot be null at this point");
      while (this.disposed < 1)
      {
        this.innerHandle.WaitOne();
        if (this.disposed == -1)
        {
          Log.Info("SingleInstanceMonitor: Event from foreign process received", this);
          this.NotifyAboutForeignEvent();
        }
        //this.disposed == 0. Disposing in progress.
        //Value cannot be 1, because while() condition cannot allow this.
        else
        {
          Log.Info("SingleInstanceMonitor: disposing", this);
          this.innerHandle.Close();
          this.disposed = 1;
          this.disposedWaitHandle.Set();
        }
      }
      Assert.IsTrue(this.innerHandle.SafeWaitHandle.IsClosed, "Handle should be closed at this point");
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
      if (!this.innerHandle.SafeWaitHandle.IsClosed)
      {
        this.innerHandle.Close();
      }
      this.disposed = 1;
      this.disposedWaitHandle.Set();
    }
  }

  protected virtual void NotifyAboutForeignEvent()
  {
    Task.Factory.StartNew(this.OnAttemptFromAnotherProcess);
  }

  protected virtual void OnAttemptFromAnotherProcess()
  {
    EventHandler handler = this.AttemptFromAnotherProcess;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  protected virtual void StartAwaitingLoop()
  {
    var checkingThread = new Thread(this.ForeignEventAwaitingLoop) { IsBackground = true };
    checkingThread.Start();
  }
}