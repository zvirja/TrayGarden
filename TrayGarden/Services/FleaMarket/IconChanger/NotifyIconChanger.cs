using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;

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

        public int DefaultDelayMsec { get; set; }

        public NotifyIconChanger()
        {
            DefaultDelayMsec = 400;
        }

        public virtual void Initialize([NotNull] NotifyIcon operableNIcon)
        {
            if (operableNIcon == null) throw new ArgumentNullException("operableNIcon");
            OperableNIcon = operableNIcon;
            BackIcon = OperableNIcon.Icon;
            Initialized = true;
        }

        public virtual void SetIcon(Icon newIcon, int msTimeout)
        {
            AssertInitialized();
            SetIconInternal(newIcon, BackIcon, msTimeout);
        }

        public virtual void SetIcon(Icon newIcon)
        {
            AssertInitialized();
            SetIcon(newIcon,DefaultDelayMsec);
        }
        

        protected virtual void SetIconInternal(Icon newIcon, Icon backIcon, int msTimeout)
        {
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
