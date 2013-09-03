using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.NotificationIcon;
using JetBrains.Annotations;
using TrayGarden.Helpers;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class ResolveNotifyIconChanger : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      if (args.ClipboardEvent)
      {
        args.CurrentNotifyIconChangerClient = NotifyIconManager.ActualManager.GlobalNotifyIconChangerClient;
      }
      else
      {
        args.CurrentNotifyIconChangerClient = NotifyIconManager.ActualManager.NotifyIconChangerClient;
        
      }
    }
  }
}
