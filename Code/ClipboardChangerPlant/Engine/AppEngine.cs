using System;
using System.Threading.Tasks;

using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.RequestHandling;

using JetBrains.Annotations;

namespace ClipboardChangerPlant.Engine;

[UsedImplicitly]
public class AppEngine
{
  private static Lazy<AppEngine> _actualEngine = new Lazy<AppEngine>(() => Factory.ActualFactory.GetApplicationEngine());

  public static AppEngine ActualEngine
  {
    get
    {
      return _actualEngine.Value;
    }
  }

  public ProcessManager RequestProcessManager { get; set; }

  public virtual void PostInit()
  {
    ClipboardManager.Provider.PostInit();
    RequestHandlerChief.PostInit();
    this.RequestProcessManager = Factory.ActualFactory.GetRequestProcessManager();
    var notifyManager = Factory.ActualFactory.GetNotifyIconManager();
    notifyManager.MainActionRequested += this.NotifyManagerOnMainActionRequested;
    notifyManager.ShorteningRequested += this.NotifyManagerOnShorteningRequested;
    ClipboardManager.Provider.OnClipboardValueChanged += this.ProviderOnOnClipboardValueChanged;
    Task.Factory.StartNew(() => ClipboardManager.Provider.OnClipboardValueUpdatedService(ClipboardManager.GetValue()));
  }

  public virtual void PreInit()
  {
    RequestHandlerChief.PreInit();
  }

  private void NotifyManagerOnMainActionRequested(object o)
  {
    this.RequestProcessManager.ProcessRequest(false, false, null, false);
  }

  private void NotifyManagerOnShorteningRequested(object o)
  {
    this.RequestProcessManager.ProcessRequest(true, false, null, false);
  }

  private void ProviderOnOnClipboardValueChanged(string s)
  {
    this.RequestProcessManager.ProcessRequest(false, true, s, false);
  }
}