using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Configuration;

using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline;

[UsedImplicitly]
public class ExecuteHandler : Processor
{
  public override void Process(ProcessorArgs args)
  {
    RequestHandler resolvedHandler = args.ResolvedHandler;
    if (resolvedHandler == null)
    {
      throw new ArgumentException("args.ResolvedHandler");
    }

    string result;
    var notifyIconManager = Factory.ActualFactory.GetNotifyIconManager();
    args.CurrentNotifyIconChangerClient.SetIcon(notifyIconManager.InProgressTrayIcon, 10000000);

    if (!resolvedHandler.TryProcess(args.ResultUrl, out result))
    {
      this.HandleErrorAndAbortPipeline(args, this.ErrorTrayIcon);
      return;
    }
    args.ShouldBeShorted = resolvedHandler.IsShorterEnabled;
    args.ResultUrl = result;
  }
}