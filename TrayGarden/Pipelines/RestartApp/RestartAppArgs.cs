using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.RestartApp
{
    public class RestartAppArgs:PipelineArgs
    {
        public string[] ParamsToAdd { get; set; }

        public RestartAppArgs(string[] paramsToAdd)
        {
            ParamsToAdd = paramsToAdd;
        }

    }
}
