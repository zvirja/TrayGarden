#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

#endregion

namespace SmallApplicationLauncher
{
  public class ContextMenuExtender : TrayGarden.Reception.Services.IExtendsGlobalMenu
  {
    #region Public Methods and Operators

    public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
    {
      foreach (KeyValuePair<string, string> application in UserConfiguration.Configuration.Applications)
      {
        KeyValuePair<string, string> app = application;
        FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(app.Value);
        menuAppender.AppentMenuStripItem(
          fileVersion.ProductName,
          Icon.ExtractAssociatedIcon(app.Value),
          (sender, args) => Process.Start(app.Value));
      }

      return true;
    }

    #endregion
  }
}