#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.MainWindow;

#endregion

namespace TrayGarden.Pipelines.Startup
{
  public class OpenConfigDiaglogIfNeed
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public void Process(StartupArgs args)
    {
      if (
        args.StartupParams.Any(
          x => x.Equals(StringConstants.OpenConfigDialogStartupKey, StringComparison.OrdinalIgnoreCase)))
      {
        this.SilentlyTryToOpenConfigurationWindow();
      }
    }

    #endregion

    #region Methods

    protected virtual void SilentlyTryToOpenConfigurationWindow()
    {
      HatcherGuide<IMainWindowDisplayer>.Instance.PopupMainWindow();
    }

    #endregion
  }
}