using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.Properties;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace ClipboardChangerPlant.NotificationIcon
{
  public class GlobalMenuExtender : TrayGarden.Reception.Services.IExtendsGlobalMenu
  {
    public static GlobalMenuExtender ActualExtender = new GlobalMenuExtender();

    public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
    {
      menuAppender.AppentMenuStripItem("Short clipboard url", Resources.klipperShortedv5, ShortUrl);
      menuAppender.AppentMenuStripItem("Handle clipboard value (Clipboard changer)",Resources.processClipboard,HandleClipboard);
      return true;
    }

    protected virtual void ShortUrl(object sender, EventArgs eventArgs)
    {
      Factory.ActualFactory.GetRequestProcessManager().ProcessRequest(true, false, null, true);
    }

    protected virtual void HandleClipboard(object sender, EventArgs e)
    {
      Factory.ActualFactory.GetRequestProcessManager().ProcessRequest(false, false, null, true);
    }
  }
}
