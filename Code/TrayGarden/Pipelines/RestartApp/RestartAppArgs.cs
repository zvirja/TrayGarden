using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.RestartApp
{
  public class RestartAppArgs : PipelineArgs
  {
    public RestartAppArgs(string[] paramsToAdd)
    {
      this.ParamsToAdd = paramsToAdd;
    }

    public string[] ParamsToAdd { get; set; }
  }
}