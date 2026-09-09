using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.RestartApp;

public class SimpleAppRestart : IPipelineProcessor<RestartAppArgs>
{
  [UsedImplicitly]
  public virtual void Process(RestartAppArgs args)
  {
    var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule!.FileName;
    System.Diagnostics.Process.Start(exePath, string.Join(" ", args.ParamsToAdd));

    Application.Current.Shutdown();
  }
}
