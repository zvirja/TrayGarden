﻿using System;
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
      HandleErrorAndAbortPipeline(args, ErrorTrayIcon);
      return;
    }
    args.ShouldBeShorted = resolvedHandler.IsShorterEnabled;
    args.ResultUrl = result;
  }
}