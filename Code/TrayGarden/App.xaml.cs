using System.Windows;
using System.Windows.Forms;

using Microsoft.Extensions.Configuration;

using Serilog;

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

    var configuration = new ConfigurationBuilder()
      .SetBasePath(System.AppContext.BaseDirectory)
      .AddJsonFile("appsettings.json", optional: false)
      .Build();

    Serilog.Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(configuration)
      .CreateLogger();

    System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

    LifecycleObserver.NotifyStartup(e.Args);
  }

  protected override void OnExit(ExitEventArgs e)
  {
    Serilog.Log.CloseAndFlush();

    base.OnExit(e);
  }
}
