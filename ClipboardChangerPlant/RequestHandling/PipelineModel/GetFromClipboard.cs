using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;
using TrayGarden.Helpers;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class GetFromClipboard : Processor
  {
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
          HandleErrorAndAbortPipeline(args, NotFoundTrayIcon);
        }
        else
          args.ResultUrl = currentValue;
      }
    }
  }
}
