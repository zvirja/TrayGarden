using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Shutdown;
using TrayGarden.Pipelines.Startup;

namespace TrayGarden.LifeCycle
{
    public class LifecycleObserver
    {

        protected static LifecycleObserver Observer { get; set; }

        public static void NotifyStartup()
        {
            if (Observer != null)
                return;
            Observer = new LifecycleObserver();
            Observer.NotifyStartupInternal();
        }


        protected virtual void NotifyStartupInternal()
        {
            try
            {
                Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
                StartupPipeline.Run();
                Application.Current.Exit += ApplicationExit;
            }
            catch (Exception ex)
            {
                Log.Error("Error at startup", ex, this);
                Application.Current.Shutdown(1);
            }
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error("Thrown exception wasn't catched. Application will be closed", e.Exception, typeof(Application));
            e.Handled = true;
            Application.Current.Shutdown(1);
        }

        protected virtual void ApplicationExit(object sender, ExitEventArgs e)
        {
            if (e.ApplicationExitCode == 0)
                ShutdownPipeline.Run();
        }


    }
}
