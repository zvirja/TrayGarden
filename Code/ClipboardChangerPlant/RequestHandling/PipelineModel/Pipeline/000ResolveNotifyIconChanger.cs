using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.NotificationIcon;

using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class ResolveNotifyIconChanger : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      NotifyIconManager notifyIconManager = NotifyIconManager.ActualManager;
      if (args.ClipboardEvent)
      {
        args.CurrentNotifyIconChangerClient = notifyIconManager.GlobalNotifyIconChangerClient;
      }
      else
      {
        args.CurrentNotifyIconChangerClient = args.OriginatorIsGlobalIcon
                                                ? notifyIconManager.GlobalNotifyIconChangerClient
                                                : notifyIconManager.NotifyIconChangerClient;
      }
    }
  }
}