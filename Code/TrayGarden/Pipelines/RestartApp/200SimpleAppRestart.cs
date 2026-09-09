using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.RestartApp;

public class SimpleAppRestart : IPipelineProcessor<RestartAppArgs>
{
  [UsedImplicitly]
  public virtual void Process(RestartAppArgs args)
  {
    var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule!.FileName;
    Log.For(this).Information("SimpleAppRestart: starting child '{ExePath}' with args '{Args}'", exePath, string.Join(" ", args.ParamsToAdd));
    System.Diagnostics.Process.Start(exePath, string.Join(" ", args.ParamsToAdd));

    Log.For(this).Information("SimpleAppRestart: shutting current process down");
    Application.Current.Shutdown();
  }
}
