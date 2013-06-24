using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.Services.FleaMarket.IconChanger
{
    public class NotifyIconChanger : INotifyIconChangerMaster
    {
        protected bool Initialized { get; set; }
        protected NotifyIcon OperableNIcon { get; set; }

        //for icon changing
        protected Task _currentUpdateIconTask;
        protected CancellationTokenSource _currentCancellationTokenSource;
        protected Icon BackIcon { get; set; }

        public bool IsEnabled { get; set; }
        public int DefaultDelayMsec { get; set; }

        public NotifyIconChanger()
        {
            DefaultDelayMsec = 400;
            IsEnabled = true;
        }

        public virtual void Initialize([NotNull] NotifyIcon operableNIcon)
        {
            Assert.ArgumentNotNull(operableNIcon, "operableNIcon");
            OperableNIcon = operableNIcon;
            BackIcon = OperableNIcon.Icon;
            Initialized = true;
        }

        public virtual void SetIcon([NotNull] Icon newIcon, int msTimeout)
        {
            Assert.ArgumentNotNull(newIcon, "newIcon");
            AssertInitialized();
            if (!IsEnabled)
                return;
            SetIconInternal(newIcon, BackIcon, msTimeout);
        }

        public virtual void SetIcon([NotNull] Icon newIcon)
        {
            Assert.ArgumentNotNull(newIcon, "newIcon");
            AssertInitialized();
            if (!IsEnabled)
                return;
            SetIcon(newIcon,DefaultDelayMsec);
        }
        

        protected virtual void SetIconInternal(Icon newIcon, Icon backIcon, int msTimeout)
        {
            if (newIcon == null || backIcon == null)
                return;
            if (_currentUpdateIconTask != null && !_currentUpdateIconTask.IsCompleted)
                _currentCancellationTokenSource.Cancel();
            _currentCancellationTokenSource = new CancellationTokenSource();
            _currentUpdateIconTask = Task.Factory.StartNew(() =>
            {
                var cancellationToken = _currentCancellationTokenSource.Token;
                OperableNIcon.Icon = newIcon;
                Thread.Sleep(msTimeout);
                if (!cancellationToken.IsCancellationRequested)
                    OperableNIcon.Icon = backIcon;
            }, _currentCancellationTokenSource.Token);
        }

        protected virtual void AssertInitialized()
        {
            if (!Initialized)
                throw new NonInitializedException();
        }
    }
}
