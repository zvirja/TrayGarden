using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class StoreOriginalUrlValue : Processor
  {
    public override void Process(ProcessorArgs args)
    {
      args.OriginalUrl = args.ResultUrl;
    }
  }
}
