using System.Windows;

using JetBrains.Annotations;

namespace TrayGarden.Pipelines.RestartApp;

public class SimpleAppRestart
{
  [UsedImplicitly]
  public virtual void Process(RestartAppArgs args)
  {
    var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule!.FileName;
    System.Diagnostics.Process.Start(exePath, string.Join(" ", args.ParamsToAdd));
    
    Application.Current.Shutdown();
  }
}