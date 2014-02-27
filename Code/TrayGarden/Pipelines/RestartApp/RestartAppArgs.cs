#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;

#endregion

namespace TrayGarden.Pipelines.RestartApp
{
  public class RestartAppArgs : PipelineArgs
  {
    #region Constructors and Destructors

    public RestartAppArgs(string[] paramsToAdd)
    {
      this.ParamsToAdd = paramsToAdd;
    }

    #endregion

    #region Public Properties

    public string[] ParamsToAdd { get; set; }

    #endregion
  }
}