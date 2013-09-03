using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.Properties;

namespace ClipboardChangerPlant.NotificationIcon
{
  public class GlobalMenuExtender : TrayGarden.Reception.Services.IExtendsGlobalMenu
  {
    public static GlobalMenuExtender ActualExtender = new GlobalMenuExtender();

    public bool GetMenuStripItemData(out string text, out Icon icon, out EventHandler clickHandler)
    {
      text = "Short clipboard url";
      icon = Resources.klipperShortedv5;
      clickHandler = ShortUrl;
      return true;
    }

    protected virtual void ShortUrl(object sender, EventArgs eventArgs)
    {
      Factory.ActualFactory.GetRequestProcessManager().ProcessRequest(true, false, null, true);
    }
  }
}
