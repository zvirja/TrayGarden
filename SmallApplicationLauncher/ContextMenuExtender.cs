using System.Collections.Generic;
using System.Diagnostics;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace SmallApplicationLauncher
{
  using System.Drawing;

  public class ContextMenuExtender : TrayGarden.Reception.Services.IExtendsGlobalMenu
  {
    public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
    {
      foreach (KeyValuePair<string, string> application in UserConfiguration.Configuration.Applications)
      {
        KeyValuePair<string, string> app = application;
        menuAppender.AppentMenuStripItem(application.Key, Icon.ExtractAssociatedIcon(app.Value), (sender, args) => Process.Start(app.Value));
      }

      return true;
    }
  }
}