using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;
using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class UpdateSuccessTrayIcon : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      var notifyIconManager = Factory.ActualFactory.GetNotifyIconManager();
      args.CurrentNotifyIconChangerClient.SetIcon(
        args.ResolvedHandler != null ? args.ResolvedHandler.DefaultHandlerIcon : SuccessTrayIcon, 1500);
    }
  }
}
