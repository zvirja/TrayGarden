using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace SmallApplicationLauncher;

public class ContextMenuExtender : TrayGarden.Reception.Services.IExtendsGlobalMenu
{
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
}