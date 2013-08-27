using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class SetToClipboard : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      ClipboardManager.SetValue(args.ResultUrl);
      var notifyIconManager = Factory.ActualFactory.GetNotifyIconManager();
      notifyIconManager.SetNewIcon(args.ResolvedHandler.HandlerIcon, 800);
    }
  }
}
