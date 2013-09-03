using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class PreExecuteHandler : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      RequestHandler resolvedHandler = args.ResolvedHandler;
      if (resolvedHandler == null)
        throw new ArgumentException("args.ResolvedHandler");

      
      if (!resolvedHandler.PreExecute(args.ResultUrl, args.ClipboardEvent))
      {
        if(args.ClipboardEvent)
          args.Abort();
        else
          HandleErrorAndAbortPipeline(args, NotFoundTrayIcon);
      }
    }
  }
}
