using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Clipboard;
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

    public ProcessManager RequestProcessManager { get; set; }

    public virtual void PreInit()
    {
      ClipboardManager.Provider.PreInit();
      RequestHandlerChief.PreInit();
    }

    public virtual void PostInit()
    {
      RequestHandlerChief.PostInit();
      this.RequestProcessManager = Factory.ActualFactory.GetRequestProcessManager();
      var notifyManager = Factory.ActualFactory.GetNotifyIconManager();
      notifyManager.MainActionRequested += NotifyManagerOnMainActionRequested;
      notifyManager.ShorteningRequested += NotifyManagerOnShorteningRequested;
      ClipboardManager.Provider.PostInit();
      ClipboardManager.Provider.OnClipboardValueChanged += ProviderOnOnClipboardValueChanged;
    }

    private void NotifyManagerOnShorteningRequested(object o)
    {
      RequestProcessManager.ProcessRequest(true, false, null, false);
    }

    private void NotifyManagerOnMainActionRequested(object o)
    {
      RequestProcessManager.ProcessRequest(false, false, null, false);
    }

    private void ProviderOnOnClipboardValueChanged(string s)
    {
      RequestProcessManager.ProcessRequest(false, true, s, false);
    }
  }
}
