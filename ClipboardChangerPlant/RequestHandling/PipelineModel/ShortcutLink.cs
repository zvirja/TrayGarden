using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Shortening;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class ShortcutLink : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      if (!args.ShouldBeShorted && !args.OnlyShorteningRequired)
        return;
      string outputString;
      if (!ShortenerManager.TryShorterUrl(args.ResultUrl, out outputString))
        HandleErrorAndAbortPipeline(args, ErrorTrayIcon);
      else
        args.ResultUrl = outputString;
    }
  }
}
