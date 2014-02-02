#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core;
using TrayGarden.TypesHatcher;

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
      var serviceInstance =
        HatcherGuide<IServicesSteward>.Instance.Services.FirstOrDefault(
          x => x.GetType().IsAssignableFrom(typeof(GlobalMenuService))) as GlobalMenuService;
      if (serviceInstance == null)
      {
        Log.Warn("Was unable to run startup configuration window", this);
      }
      else
      {
        serviceInstance.ManuallyOpenConfigurationWindow();
      }
    }

    #endregion
  }
}