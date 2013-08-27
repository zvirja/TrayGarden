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
      if (!ReqHandlerResolver.TryToResolveHandler(args.ResultUrl, out resolvedHandler))
      {
        HandleError(args, NotFoundTrayIcon);
        return;
      }
      args.ResolvedHandler = resolvedHandler;
      string result;
      var notifyIconManager = Factory.ActualFactory.GetNotifyIconManager();
      notifyIconManager.SetNewIcon(notifyIconManager.InProgressTrayIcon, 30000);
      if (!resolvedHandler.TryProcess(args.ResultUrl, out result))
      {
        HandleError(args, ErrorTrayIcon);
        return;
      }
      args.ShouldBeShorted = resolvedHandler.IsShorterEnabled;
      args.ResultUrl = result;
    }
  }
}
