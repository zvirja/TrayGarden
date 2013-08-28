using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class UpdateSuccessTrayIcon : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      var notifyIconManager = Factory.ActualFactory.GetNotifyIconManager();
      notifyIconManager.SetNewIcon(args.ResolvedHandler != null ? args.ResolvedHandler.HandlerIcon : SuccessTrayIcon, 800);
    }
  }
}
