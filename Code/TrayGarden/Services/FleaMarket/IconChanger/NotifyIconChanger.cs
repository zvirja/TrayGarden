using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Resources;

namespace TrayGarden.Services.FleaMarket.IconChanger;

public class NotifyIconChanger : INotifyIconChangerMaster
{
  protected CancellationTokenSource _currentCancellationTokenSource;

  protected Task _currentUpdateIconTask;

  public NotifyIconChanger()
  {
    DefaultDelayMsec = 400;
    IsEnabled = true;
  }

  public int DefaultDelayMsec { get; set; }

  public bool IsEnabled { get; set; }

  protected Icon BackIcon { get; set; }

  protected Icon SuccessIcon { get; set; }
    
  protected Icon FailedIcon { get; set; }

  protected bool Initialized { get; set; }

  protected NotifyIcon OperableNIcon { get; set; }

  public virtual void Initialize([NotNull] NotifyIcon operableNIcon)
  {
    Assert.ArgumentNotNull(operableNIcon, "operableNIcon");
    OperableNIcon = operableNIcon;
    BackIcon = OperableNIcon.Icon;
    SuccessIcon = GlobalResourcesManager.GetIconByName("mockAction");
    FailedIcon = GlobalResourcesManager.GetIconByName("mockActionFailed");
    Initialized = true;
  }

  public void NotifySuccess(int msTimeout = 0)
  {
    SetIcon(SuccessIcon, msTimeout == 0 ? DefaultDelayMsec : msTimeout);
  }
    
  public void NotifyFailed(int msTimeout = 0)
  {
    SetIcon(FailedIcon, msTimeout == 0 ? DefaultDelayMsec : msTimeout);
  }

  public virtual void SetIcon(Icon newIcon, int msTimeout)
  {
    AssertInitialized();
    if (!IsEnabled)
    {
      return;
    }
    SetIconInternal(newIcon ?? BackIcon, BackIcon, msTimeout);
  }

  public virtual void SetIcon(Icon newIcon)
  {
    AssertInitialized();
    if (!IsEnabled)
    {
      return;
    }
    SetIcon(newIcon, DefaultDelayMsec);
  }

  protected virtual void AssertInitialized()
  {
    if (!Initialized)
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
    if (_currentUpdateIconTask != null && !_currentUpdateIconTask.IsCompleted)
    {
      _currentCancellationTokenSource.Cancel();
    }
    _currentCancellationTokenSource = new CancellationTokenSource();
    _currentUpdateIconTask = Task.Factory.StartNew(
      () =>
      {
        var cancellationToken = _currentCancellationTokenSource.Token;
        OperableNIcon.Icon = newIcon;
        Thread.Sleep(msTimeout);
        if (!cancellationToken.IsCancellationRequested)
        {
          OperableNIcon.Icon = backIcon;
        }
      },
      _currentCancellationTokenSource.Token);
  }
}