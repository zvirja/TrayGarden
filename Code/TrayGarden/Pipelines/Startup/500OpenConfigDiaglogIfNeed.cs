using System;
using System.Linq;
using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.MainWindow;

namespace TrayGarden.Pipelines.Startup;

public class OpenConfigDiaglogIfNeed
{
  [UsedImplicitly]
  public void Process(StartupArgs args)
  {
    if (args.StartupParams.Any(x => x.Equals(StringConstants.OpenConfigDialogStartupKey, StringComparison.OrdinalIgnoreCase)))
    {
      this.SilentlyTryToOpenConfigurationWindow();
    }
  }

  protected virtual void SilentlyTryToOpenConfigurationWindow()
  {
    HatcherGuide<IMainWindowDisplayer>.Instance.PopupMainWindow();
  }
}