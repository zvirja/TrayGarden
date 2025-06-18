using System.Windows;

using TrayGarden.LifeCycle;

namespace TrayGarden;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  protected override void OnStartup(StartupEventArgs e)
  {
    base.OnStartup(e);
    LifecycleObserver.NotifyStartup(e.Args);
  }
}