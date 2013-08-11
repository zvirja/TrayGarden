using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace TrayGarden.Pipelines.RestartApp
{
    public class SimpleAppRestart
    {
        public virtual void Process(RestartAppArgs args)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location, string.Join(" ", args.ParamsToAdd));
            Application.Current.Shutdown();
        }
    }
}
