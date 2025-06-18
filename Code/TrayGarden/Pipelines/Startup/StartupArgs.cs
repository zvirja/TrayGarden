using System.Windows;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.Startup;

public class StartupArgs : PipelineArgs
{
  public StartupArgs(string[] startParams)
  {
    StartupParams = startParams;
  }

  public Application App
  {
    get
    {
      return Application.Current;
    }
  }

  public string[] StartupParams { get; set; }
}