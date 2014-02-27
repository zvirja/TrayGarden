#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.FleaMarket.IconChanger;

#endregion

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class ProcessorArgs
  {
    #region Constructors and Destructors

    public ProcessorArgs(bool onlyShorteningRequired, bool clipboardEvent, string predefinedClipboardValue, bool originatorIsGlobalIcon)
    {
      this.OnlyShorteningRequired = onlyShorteningRequired;
      this.ClipboardEvent = clipboardEvent;
      this.PredefinedClipboardValue = predefinedClipboardValue;
      this.OriginatorIsGlobalIcon = originatorIsGlobalIcon;
    }

    #endregion

    #region Public Properties

    public bool Aborted { get; protected set; }

    public bool ClipboardEvent { get; set; }

    public INotifyIconChangerClient CurrentNotifyIconChangerClient { get; set; }

    public bool OnlyShorteningRequired { get; set; }

    public string OriginalUrl { get; set; }

    public bool OriginatorIsGlobalIcon { get; set; }

    public string PredefinedClipboardValue { get; set; }

    public RequestHandler ResolvedHandler { get; set; }

    public string ResultUrl { get; set; }

    public bool ShouldBeShorted { get; set; }

    #endregion

    #region Public Methods and Operators

    public void Abort()
    {
      this.Aborted = true;
    }

    #endregion
  }
}