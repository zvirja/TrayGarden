#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Pipelines.RestartApp
{
  public class SimpleAppRestart
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(RestartAppArgs args)
    {
      System.Diagnostics.Process.Start(Application.ResourceAssembly.Location, string.Join(" ", args.ParamsToAdd));
      Application.Current.Shutdown();
    }

    #endregion
  }
}