using System;
using System.Linq;

using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.MainWindow;

namespace TrayGarden.Pipelines.Startup;

public class OpenConfigDiaglogIfNeed(IMainWindowDisplayer mainWindowDisplayer) : IPipelineProcessor<StartupArgs>
{
  [UsedImplicitly]
  public void Process(StartupArgs args)
  {
    if (args.StartupParams.Any(x => x.Equals(StringConstants.OpenConfigDialogStartupKey, StringComparison.OrdinalIgnoreCase)))
    {
      SilentlyTryToOpenConfigurationWindow();
    }
  }

  protected virtual void SilentlyTryToOpenConfigurationWindow()
  {
    mainWindowDisplayer.PopupMainWindow();
  }
}
