#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.MainWindow;

#endregion

namespace TrayGarden.Pipelines.Startup
{
  public class SingleInstanceCheckAndHooks
  {
    #region Properties

    protected SynchronizationContext UISynchronizationContext { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public void Process(StartupArgs args)
    {
      this.UISynchronizationContext = SynchronizationContext.Current;

      var monitor = HatcherGuide<ISingleInstanceMonitor>.Instance;
      bool isFirstInstance = monitor.TryAcquireOwnershipNotifyIfFail();
      if (!isFirstInstance)
      {
        Application.Current.Shutdown(2);
        args.Abort();
      }
      else
      {
        monitor.AttemptFromAnotherProcess +=
          delegate(object sender, EventArgs eventArgs)
            {
              this.UISynchronizationContext.Post(this.OpenConfigurationWindow, null);
            };
      }
    }

    #endregion

    #region Methods

    protected virtual void OpenConfigurationWindow(object obj)
    {
      HatcherGuide<IMainWindowDisplayer>.Instance.PopupMainWindow();
    }

    #endregion
  }
}