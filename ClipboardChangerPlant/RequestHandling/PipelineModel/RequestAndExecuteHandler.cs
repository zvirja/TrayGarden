using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class RequestAndExecuteHandler : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      RequestHandler resolvedHandler;
      if (!RequestHandlerChief.TryToResolveHandler(args.ResultUrl, out resolvedHandler))
      {
        if (!args.ClipboardEvent)
          HandleErrorAndAbortPipeline(args, NotFoundTrayIcon);
        else
          args.Abort();
        return;
      }
      args.ResolvedHandler = resolvedHandler;

      string result;
      var notifyIconManager = Factory.ActualFactory.GetNotifyIconManager();
      notifyIconManager.SetNewIcon(notifyIconManager.InProgressTrayIcon, 10000000);

      if (!resolvedHandler.TryProcess(args.ResultUrl, out result))
      {
        HandleErrorAndAbortPipeline(args, ErrorTrayIcon);
        return;
      }
      args.ShouldBeShorted = resolvedHandler.IsShorterEnabled;
      args.ResultUrl = result;
    }
  }
}
