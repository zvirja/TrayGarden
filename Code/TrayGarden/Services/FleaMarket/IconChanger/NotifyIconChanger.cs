﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Resources;

namespace TrayGarden.Services.FleaMarket.IconChanger
{
  public class NotifyIconChanger : INotifyIconChangerMaster
  {
    protected CancellationTokenSource _currentCancellationTokenSource;

    protected Task _currentUpdateIconTask;

    public NotifyIconChanger()
    {
      this.DefaultDelayMsec = 400;
      this.IsEnabled = true;
    }

    public int DefaultDelayMsec { get; set; }

    public bool IsEnabled { get; set; }

    protected Icon BackIcon { get; set; }

    protected Icon SuccessIcon { get; set; }

    protected bool Initialized { get; set; }

    protected NotifyIcon OperableNIcon { get; set; }

    public virtual void Initialize([NotNull] NotifyIcon operableNIcon)
    {
      Assert.ArgumentNotNull(operableNIcon, "operableNIcon");
      this.OperableNIcon = operableNIcon;
      this.BackIcon = this.OperableNIcon.Icon;
      this.SuccessIcon = GlobalResourcesManager.GetIconByName("mockAction");
      this.Initialized = true;
    }

    public void NotifySuccess(int msTimeout = 0)
    {
      this.SetIcon(this.SuccessIcon, msTimeout == 0 ? this.DefaultDelayMsec : msTimeout);
    }

    public virtual void SetIcon(Icon newIcon, int msTimeout)
    {
      this.AssertInitialized();
      if (!this.IsEnabled)
      {
        return;
      }
      this.SetIconInternal(newIcon ?? this.BackIcon, this.BackIcon, msTimeout);
    }

    public virtual void SetIcon(Icon newIcon)
    {
      this.AssertInitialized();
      if (!this.IsEnabled)
      {
        return;
      }
      this.SetIcon(newIcon, this.DefaultDelayMsec);
    }

    protected virtual void AssertInitialized()
    {
      if (!this.Initialized)
      {
        throw new NonInitializedException();
      }
    }

    protected virtual void SetIconInternal(Icon newIcon, Icon backIcon, int msTimeout)
    {
      if (newIcon == null || backIcon == null)
      {
        return;
      }
      if (this._currentUpdateIconTask != null && !this._currentUpdateIconTask.IsCompleted)
      {
        this._currentCancellationTokenSource.Cancel();
      }
      this._currentCancellationTokenSource = new CancellationTokenSource();
      this._currentUpdateIconTask = Task.Factory.StartNew(
        () =>
          {
            var cancellationToken = this._currentCancellationTokenSource.Token;
            this.OperableNIcon.Icon = newIcon;
            Thread.Sleep(msTimeout);
            if (!cancellationToken.IsCancellationRequested)
            {
              this.OperableNIcon.Icon = backIcon;
            }
          },
        this._currentCancellationTokenSource.Token);
    }
  }
}