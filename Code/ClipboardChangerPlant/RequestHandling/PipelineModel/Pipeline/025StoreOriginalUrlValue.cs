#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class StoreOriginalUrlValue : Processor
  {
    #region Public Methods and Operators

    public override void Process(ProcessorArgs args)
    {
      args.OriginalUrl = args.ResultUrl;
    }

    #endregion
  }
}