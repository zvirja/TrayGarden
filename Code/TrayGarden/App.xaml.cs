using System.Windows;
using System.Windows.Forms;
using TrayGarden.LifeCycle;
using Application = System.Windows.Application;

namespace TrayGarden;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  protected override void OnStartup(StartupEventArgs e)
  {
    base.OnStartup(e);

    System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
    
    LifecycleObserver.NotifyStartup(e.Args);
  }
}