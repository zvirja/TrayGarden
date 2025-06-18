using System.Windows;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.Shutdown;

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