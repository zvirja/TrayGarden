using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class ProcessorArgs
  {
    public bool Aborted { get; protected set; }
    public string ResultUrl { get; set; }
    public bool ShouldBeShorted { get; set; }
    public RequestHandler ResolvedHandler { get; set; }

    public bool OnlyShorteningRequired { get; set; }
    public string PredefinedClipboardValue { get; set; }
    public bool ClipboardEvent { get; set; }


    public ProcessorArgs(bool onlyShorteningRequired, bool clipboardEvent, string predefinedClipboardValue)
    {
      OnlyShorteningRequired = onlyShorteningRequired;
      ClipboardEvent = clipboardEvent;
      PredefinedClipboardValue = predefinedClipboardValue;
    }

    public void Abort()
    {
      Aborted = true;
    }
  }
}
