using System;
using System.Threading;
using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.MainWindow;

namespace TrayGarden.Pipelines.Startup;

public class SingleInstanceCheckAndHooks(ISingleInstanceMonitor monitor, IMainWindowDisplayer mainWindowDisplayer)
  : IPipelineProcessor<StartupArgs>
{
  private SynchronizationContext UISynchronizationContext { get; set; }

  [UsedImplicitly]
  public void Process(StartupArgs args)
  {
    UISynchronizationContext = SynchronizationContext.Current;

    bool isFirstInstance = monitor.TryAcquireOwnershipNotifyIfFail();
    if (!isFirstInstance)
    {
      Application.Current.Shutdown(2);
      args.Abort();
    }
    else
    {
      monitor.AttemptFromAnotherProcess +=
        delegate(object sender, EventArgs eventArgs) { UISynchronizationContext.Post(OpenConfigurationWindow, null); };
    }
  }

  private void OpenConfigurationWindow(object obj)
  {
    mainWindowDisplayer.PopupMainWindow();
  }
}
