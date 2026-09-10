using System;
using ClipboardChangerPlant.Clipboard;

using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline;

[UsedImplicitly]
public class PostmortemRevertValueHandler : Processor
{
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
}