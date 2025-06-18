using System.Windows;

using JetBrains.Annotations;

namespace TrayGarden.Pipelines.RestartApp;

public class SimpleAppRestart
{
  [UsedImplicitly]
  public virtual void Process(RestartAppArgs args)
  {
    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location, string.Join(" ", args.ParamsToAdd));
    Application.Current.Shutdown();
  }
}