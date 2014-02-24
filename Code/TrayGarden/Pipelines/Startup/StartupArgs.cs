using System.Windows;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.Startup
{
  public class StartupArgs : PipelineArgs
  {
    public string[] StartupParams { get; set; }

    public Application App
    {
      get { return Application.Current; }
    }

    public StartupArgs(string[] startParams)
    {
      StartupParams = startParams;
    }
  }
}
