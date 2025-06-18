using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.FleaMarket.IconChanger;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class ProcessorArgs
  {
    public ProcessorArgs(bool onlyShorteningRequired, bool clipboardEvent, string predefinedClipboardValue, bool originatorIsGlobalIcon)
    {
      this.OnlyShorteningRequired = onlyShorteningRequired;
      this.ClipboardEvent = clipboardEvent;
      this.PredefinedClipboardValue = predefinedClipboardValue;
      this.OriginatorIsGlobalIcon = originatorIsGlobalIcon;
    }

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

    public void Abort()
    {
      this.Aborted = true;
    }
  }
}