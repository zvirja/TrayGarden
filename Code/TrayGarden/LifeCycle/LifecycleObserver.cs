using System;
using System.Linq;
using System.Reflection;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Pipelines.RestartApp;
using TrayGarden.Pipelines.Shutdown;
using TrayGarden.Pipelines.Startup;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.UI;
using TrayGarden.UI.Common;

namespace TrayGarden.LifeCycle;

public class LifecycleObserver
{
  private static LifecycleObserver Observer { get; set; }

  private static IServiceProvider Services { get; set; }

  public static void NotifyStartup(string[] args, IServiceProvider services)
  {
    if (Observer != null)
    {
      return;
    }
    Services = services;
    Observer = new LifecycleObserver();
    Observer.SetAssembliesHook();
    Observer.PopulateGardenContext();
    Observer.NotifyStartupInternal(args);
  }

  public static void RestartApp(string[] paramsToAdd)
  {
    Log.For(typeof(LifecycleObserver)).Information(
      "RestartApp requested. Extra params: {Params}. Call stack:{NewLine}{StackTrace}",
      string.Join(" ", paramsToAdd),
      Environment.NewLine,
      Environment.StackTrace);
    Services.GetRequiredService<IPipelineRunner>().Run(new RestartAppArgs(paramsToAdd));
  }

  protected virtual void PopulateGardenContext()
  {
    GardenContext.RuntimeSettings = Services.GetRequiredService<IRuntimeSettingsManager>();
    GardenContext.ServiceForPlantTemplateSelector = Services.GetRequiredService<IDataTemplateSelector>();
    GardenContext.UIManager = Services.GetRequiredService<IUIManager>();
  }

  protected virtual void ApplicationExit(object sender, ExitEventArgs e)
  {
    Log.For(this).Information("ApplicationExit. ExitCode: {ExitCode}", e.ApplicationExitCode);
    if (e.ApplicationExitCode != 0)
    {
      return;
    }
    try
    {
      Services.GetRequiredService<IPipelineRunner>().Run(new ShutdownArgs());
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Shutdown pipeline failed");
    }
  }

  protected Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
  {
    var name = new AssemblyName(args.Name);

    return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == name.Name);
  }

  protected virtual void NotifyStartupInternal(string[] args)
  {
    try
    {
      Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
      Services.GetRequiredService<IPipelineRunner>().Run(new StartupArgs(args), maskExceptions: false);
      Application.Current.Exit += ApplicationExit;
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Error at startup");
      Application.Current.Shutdown(1);
    }
  }

  protected virtual void SetAssembliesHook()
  {
    AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
  }

  private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
  {
    Log.For(typeof(Application)).Error(e.Exception, "Thrown exception wasn't catched. Application will be closed");
    e.Handled = true;
    Application.Current.Shutdown(1);
  }
}
