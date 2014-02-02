#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.WindowWithReturn;

#endregion

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline
{
  public class CreateViewModel
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetMainVMPipelineArgs args)
    {
      args.ResultVM = new WindowWithBackVM();
    }

    #endregion
  }
}