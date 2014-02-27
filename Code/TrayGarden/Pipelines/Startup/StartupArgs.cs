#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using TrayGarden.Pipelines.Engine;

#endregion

namespace TrayGarden.Pipelines.Startup
{
  public class StartupArgs : PipelineArgs
  {
    #region Constructors and Destructors

    public StartupArgs(string[] startParams)
    {
      this.StartupParams = startParams;
    }

    #endregion

    #region Public Properties

    public Application App
    {
      get
      {
        return Application.Current;
      }
    }

    public string[] StartupParams { get; set; }

    #endregion
  }
}