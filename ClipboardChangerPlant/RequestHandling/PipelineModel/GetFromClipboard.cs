using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class GetFromClipboard : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      var currentValue = Factory.ActualFactory.GetClipboardProvider().GetValue();
      if (string.IsNullOrEmpty(currentValue))
        HandleError(args, NotFoundTrayIcon);
      else
        args.ResultUrl = currentValue;

    }
  }
}
