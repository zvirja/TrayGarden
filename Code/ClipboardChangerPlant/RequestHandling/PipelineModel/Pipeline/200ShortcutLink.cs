﻿using ClipboardChangerPlant.Shortening;

using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline;

[UsedImplicitly]
public class ShortcutLink : Processor
{
  public override void Process(ProcessorArgs args)
  {
    if (!args.ShouldBeShorted && !args.OnlyShorteningRequired)
    {
      return;
    }
    string outputString;
    if (!ShortenerManager.TryShorterUrl(args.ResultUrl, out outputString))
    {
      HandleErrorAndAbortPipeline(args, ErrorTrayIcon);
    }
    else
    {
      args.ResultUrl = outputString;
    }
  }
}