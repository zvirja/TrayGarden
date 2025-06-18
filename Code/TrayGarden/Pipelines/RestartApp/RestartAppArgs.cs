using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.RestartApp;

public class RestartAppArgs : PipelineArgs
{
  public RestartAppArgs(string[] paramsToAdd)
  {
    this.ParamsToAdd = paramsToAdd;
  }

  public string[] ParamsToAdd { get; set; }
}