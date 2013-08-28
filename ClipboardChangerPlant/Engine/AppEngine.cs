using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.NotificationIcon;
using ClipboardChangerPlant.RequestHandling;
using JetBrains.Annotations;

namespace ClipboardChangerPlant.Engine
{
  [UsedImplicitly]
  public class AppEngine
  {
    private static Lazy<AppEngine> _actualEngine = new Lazy<AppEngine>(() => Factory.ActualFactory.GetApplicationEngine());
    public static AppEngine ActualEngine
    {
      get { return _actualEngine.Value; }
    }

    public NotifyIconManager NotifyIconManager { get; set; }
    public ProcessManager RequestProcessManager { get; set; }

    public virtual void Init(NotifyIcon notifyIcon)
    {
      var notifyManager = Factory.ActualFactory.GetNotifyIconManager();
      notifyManager.Initialize(notifyIcon);
      notifyManager.MainActionRequested += new Action<object>(notifyManager_MainActionRequested);
      this.RequestProcessManager = Factory.ActualFactory.GetRequestProcessManager();
    }

    private void notifyManager_MainActionRequested(object sender)
    {
      RequestProcessManager.ProcessRequest();
    }
  }
}
