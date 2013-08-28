using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.NotificationIcon
{
  public class NotifyIconManager : INeedCongurationNode
  {
    protected XmlHelper ConfigurationHelper;
    private static readonly Lazy<NotifyIconManager> _manager = new Lazy<NotifyIconManager>(() => Factory.ActualFactory.GetNotifyIconManager());
    public static NotifyIconManager ActualManager
    {
      get
      {
        return _manager.Value;
      }
    }

    private NotifyIcon _notifyIcon;

    public NotifyIcon NotifyIcon
    {
      get
      {
        if (_notifyIcon == null)
          throw new NoNullAllowedException("NotifyIconManager should be initialized");
        return _notifyIcon;
      }
      set { _notifyIcon = value; }
    }

    public void Initialize(NotifyIcon notifyIcon)
    {
      NotifyIcon = notifyIcon;
      NotifyIcon.MouseClick += new MouseEventHandler(NotifyIcon_MouseClick);
      NotifyIcon.Icon = DefaultTrayIcon;
    }

    void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
        OnMainActionRequested();
    }

    public event Action<object> MainActionRequested;

    protected virtual void OnMainActionRequested()
    {
      Action<object> handler = MainActionRequested;
      if (handler != null) handler(this);
    }

    private Task _currentUpdateIconTask;
    private CancellationTokenSource _currentCancellationTokenSource;

    public virtual void SetNewIcon(Icon newIcon, int msTimeout = 0)
    {
      if (msTimeout == 0)
        msTimeout = ConfigurationHelper.GetIntValue("DefaultTimeout");
      SetIconInternal(newIcon, DefaultTrayIcon, msTimeout);
    }

    protected virtual void SetIconInternal(Icon newIcon, Icon backIcon, int msTimeout)
    {
      if (_currentUpdateIconTask != null && !_currentUpdateIconTask.IsCompleted)
        _currentCancellationTokenSource.Cancel();
      _currentCancellationTokenSource = new CancellationTokenSource();
      _currentUpdateIconTask = Task.Factory.StartNew(() =>
                                {
                                  var cancellationToken = _currentCancellationTokenSource.Token;
                                  _notifyIcon.Icon = newIcon;
                                  Thread.Sleep(msTimeout);
                                  if (!cancellationToken.IsCancellationRequested)
                                    _notifyIcon.Icon = backIcon;
                                }, _currentCancellationTokenSource.Token);
    }

    public Icon DefaultTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("DefaultTrayIcon")); }
    }

    public Icon NotFoundTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("NotFoundTrayIcon")); }
    }

    public Icon ErrorTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("ErrorTrayIcon")); }
    }

    public Icon SuccessTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("SuccessTrayIcon")); }
    }

    public Icon InProgressTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("InProgressTrayIcon")); }
    }

    public void SetConfigurationNode(XmlNode configurationNode)
    {
      ConfigurationHelper = new XmlHelper(configurationNode);
    }

    public string Name { get; set; }
  }
}
