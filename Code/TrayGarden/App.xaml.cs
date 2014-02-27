#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using TrayGarden.LifeCycle;

#endregion

namespace TrayGarden
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    #region Methods

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      LifecycleObserver.NotifyStartup(e.Args);
    }

    #endregion
  }
}