#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.NotificationIcon;

using JetBrains.Annotations;

#endregion

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class ResolveNotifyIconChanger : Processor
  {
    #region Public Methods and Operators

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

    #endregion
  }
}