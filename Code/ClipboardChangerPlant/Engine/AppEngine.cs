#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.RequestHandling;

using JetBrains.Annotations;

#endregion

namespace ClipboardChangerPlant.Engine
{
  [UsedImplicitly]
  public class AppEngine
  {
    #region Static Fields

    private static Lazy<AppEngine> _actualEngine = new Lazy<AppEngine>(() => Factory.ActualFactory.GetApplicationEngine());

    #endregion

    #region Public Properties

    public static AppEngine ActualEngine
    {
      get
      {
        return _actualEngine.Value;
      }
    }

    public ProcessManager RequestProcessManager { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual void PostInit()
    {
      ClipboardManager.Provider.PostInit();
      RequestHandlerChief.PostInit();
      this.RequestProcessManager = Factory.ActualFactory.GetRequestProcessManager();
      var notifyManager = Factory.ActualFactory.GetNotifyIconManager();
      notifyManager.MainActionRequested += this.NotifyManagerOnMainActionRequested;
      notifyManager.ShorteningRequested += this.NotifyManagerOnShorteningRequested;
      ClipboardManager.Provider.OnClipboardValueChanged += this.ProviderOnOnClipboardValueChanged;
    }

    public virtual void PreInit()
    {
      RequestHandlerChief.PreInit();
    }

    #endregion

    #region Methods

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

    #endregion
  }
}