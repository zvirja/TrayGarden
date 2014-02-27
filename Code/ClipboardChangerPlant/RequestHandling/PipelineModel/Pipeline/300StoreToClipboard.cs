#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Clipboard;

using JetBrains.Annotations;

#endregion

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class StoreToClipboard : Processor
  {
    #region Public Methods and Operators

    public override void Process(ProcessorArgs args)
    {
      ClipboardManager.SetValue(args.ResultUrl, true);
    }

    #endregion
  }
}