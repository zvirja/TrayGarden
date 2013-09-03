using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class ResolveHandler : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      RequestHandler resolvedHandler;
      if (!RequestHandlerChief.TryToResolveHandler(args, out resolvedHandler))
      {
        if (!args.ClipboardEvent)
          HandleErrorAndAbortPipeline(args, NotFoundTrayIcon);
        else
          args.Abort();
        return;
      }
      args.ResolvedHandler = resolvedHandler;
    }
  }
}
