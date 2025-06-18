using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Clipboard;

using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class StoreToClipboard : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      ClipboardManager.SetValue(args.ResultUrl, true);
    }
  }
}