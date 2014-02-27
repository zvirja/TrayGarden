#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class ResolveHandler : Processor
  {
    #region Public Methods and Operators

    public override void Process(ProcessorArgs args)
    {
      RequestHandler resolvedHandler;
      if (!RequestHandlerChief.TryToResolveHandler(args, out resolvedHandler))
      {
        if (!args.ClipboardEvent)
        {
          this.HandleErrorAndAbortPipeline(args, this.NotFoundTrayIcon);
        }
        else
        {
          args.Abort();
        }
        return;
      }
      args.ResolvedHandler = resolvedHandler;
    }

    #endregion
  }
}