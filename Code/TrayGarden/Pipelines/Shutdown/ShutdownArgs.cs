#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using TrayGarden.Pipelines.Engine;

#endregion

namespace TrayGarden.Pipelines.Shutdown
{
  public class ShutdownArgs : PipelineArgs
  {
    #region Public Properties

    public Application App
    {
      get
      {
        return Application.Current;
      }
    }

    #endregion
  }
}