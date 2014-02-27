#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Configuration;

using JetBrains.Annotations;

using TrayGarden.Helpers;

#endregion

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class PullValueFromClipboard : Processor
  {
    #region Public Methods and Operators

    public override void Process(ProcessorArgs args)
    {
      if (args.ClipboardEvent)
      {
        if (args.PredefinedClipboardValue.IsNullOrEmpty())
        {
          args.Abort();
          return;
        }
        args.ResultUrl = args.PredefinedClipboardValue;
      }
      else
      {
        string currentValue = Factory.ActualFactory.GetClipboardProvider().GetValue();
        if (currentValue.IsNullOrEmpty())
        {
          this.HandleErrorAndAbortPipeline(args, this.NotFoundTrayIcon);
        }
        else
        {
          args.ResultUrl = currentValue;
        }
      }
    }

    #endregion
  }
}