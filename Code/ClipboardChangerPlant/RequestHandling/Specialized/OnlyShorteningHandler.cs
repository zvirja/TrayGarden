#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.RequestHandling.PipelineModel;

#endregion

namespace ClipboardChangerPlant.RequestHandling.Specialized
{
  public class OnlyShorteningHandler : RequestHandler
  {
    #region Public Methods and Operators

    public override bool? Match(ProcessorArgs args)
    {
      if (!args.OnlyShorteningRequired)
      {
        return false;
      }
      if (base.Match(args) == true)
      {
        return true;
      }
      else
      {
        return null;
      }
    }

    #endregion
  }
}