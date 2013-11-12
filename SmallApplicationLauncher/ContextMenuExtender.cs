using System.Collections.Generic;
using System.Diagnostics;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;
using System.Windows.Forms;

namespace SmallApplicationLauncher
{
  public class ContextMenuExtender : TrayGarden.Reception.Services.IExtendsGlobalMenu
  {
    public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
    {
      foreach (KeyValuePair<string, string> application in UserConfiguration.Configuration.Applications)
      {
        KeyValuePair<string, string> app = application;
        
        // YBO: When the issue #7 will be fixed, feel free to use this line instead:
        // menuAppender.AppentMenuStripItem(application.Key, null, (sender, args) => Process.Start(app.Value));

        var appender = menuAppender as MenuEntriesAppender;
        if (appender != null)
        {
          appender.OutputItems.Add(new ToolStripMenuItem(application.Key, null, (sender, args) => Process.Start(app.Value)));
        }
      }

      return true;
    }
  }
}