using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.Shutdown
{
  public class ShutdownArgs : PipelineArgs
  {
    public Application App
    {
      get
      {
        return Application.Current;
      }
    }
  }
}