#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Clipboard;

using JetBrains.Annotations;

#endregion

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class PostmortemRevertValueHandler : Processor
  {
    #region Public Methods and Operators

    public override void Process(ProcessorArgs args)
    {
      RequestHandler resolvedHandler = args.ResolvedHandler;
      if (resolvedHandler == null)
      {
        throw new ArgumentException("args.ResolvedHandler");
      }
      if (resolvedHandler.PostmortemRevertValue(args.ResultUrl, args.OriginalUrl, args.ClipboardEvent))
      {
        ClipboardManager.SetValue(args.OriginalUrl, true);
      }
    }

    #endregion
  }
}