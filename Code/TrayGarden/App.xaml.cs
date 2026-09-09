using System.Windows;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using TrayGarden.Composition;
using TrayGarden.LifeCycle;

using Application = System.Windows.Application;

namespace TrayGarden;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  private IHost _host;

  protected override void OnStartup(StartupEventArgs e)
  {
    base.OnStartup(e);

    var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
    {
      ContentRootPath = System.AppContext.BaseDirectory
    });

    builder.Services.AddSerilog((_, configuration) => configuration.ReadFrom.Configuration(builder.Configuration));
    builder.Services.AddGarden(builder.Configuration);

    _host = builder.Build();
    _host.Start();

    System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

    LifecycleObserver.NotifyStartup(e.Args, _host.Services);
  }

  protected override void OnExit(ExitEventArgs e)
  {
    _host?.Dispose();
    Serilog.Log.CloseAndFlush();

    base.OnExit(e);
  }
}
