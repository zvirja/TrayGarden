using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.RestartApp;
using TrayGarden.Pipelines.Shutdown;
using TrayGarden.Pipelines.Startup;
using TrayGarden.Resources;

namespace TrayGarden.LifeCycle
{
  public class LifecycleObserver
  {
    protected static LifecycleObserver Observer { get; set; }

    public static void NotifyStartup(string[] args)
    {
      if (Observer != null)
      {
        return;
      }
      Observer = new LifecycleObserver();
      Observer.LoadLog4NetFromResources();
      Observer.SetAssembliesHook();
      Observer.NotifyStartupInternal(args);
    }

    public static void RestartApp(string[] paramsToAdd)
    {
      RestartAppPipeline.Run(paramsToAdd);
    }

    protected virtual void ApplicationExit(object sender, ExitEventArgs e)
    {
      if (e.ApplicationExitCode == 0)
      {
        ShutdownPipeline.Run();
      }
    }

    protected Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
    {
      return
        AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith(args.Name, StringComparison.OrdinalIgnoreCase));
    }

    protected virtual void LoadLog4NetFromResources()
    {
      var alreadyLoaded = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith("log4net"));
      if (alreadyLoaded != null)
      {
        return;
      }
      byte[] log4Net = GlobalResources.log4net;
      Assembly.Load(log4Net);
    }

    protected virtual void NotifyStartupInternal(string[] args)
    {
      try
      {
        Application.Current.DispatcherUnhandledException += this.Current_DispatcherUnhandledException;
        StartupPipeline.Run(args);
        Application.Current.Exit += this.ApplicationExit;
      }
      catch (Exception ex)
      {
        Log.Error("Error at startup", ex, this);
        Application.Current.Shutdown(1);
      }
    }

    protected virtual void SetAssembliesHook()
    {
      AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomainOnAssemblyResolve;
    }

    private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      Log.Error("Thrown exception wasn't catched. Application will be closed", e.Exception, typeof(Application));
      e.Handled = true;
      Application.Current.Shutdown(1);
    }
  }
}