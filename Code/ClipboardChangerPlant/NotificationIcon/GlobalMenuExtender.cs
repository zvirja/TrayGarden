using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.Properties;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

namespace ClipboardChangerPlant.NotificationIcon;

public class GlobalMenuExtender : TrayGarden.Reception.Services.IExtendsGlobalMenu
{
  public static GlobalMenuExtender ActualExtender = new GlobalMenuExtender();

  public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
  {
    menuAppender.AppentMenuStripItem(
      "Short clipboard url",
      Resources.klipperShortedv5,
      this.ShortUrl,
      new SimpleDynamicStateProvider() { CurrentRelevanceLevel = RelevanceLevel.Low });
    menuAppender.AppentMenuStripItem("Handle clipboard value (Clipboard changer)", Resources.processClipboard, this.HandleClipboard, new IsUrlInClipboardWatcher());
    return true;
  }

  protected virtual void HandleClipboard(object sender, EventArgs e)
  {
    Factory.ActualFactory.GetRequestProcessManager().ProcessRequest(false, false, null, true);
  }

  protected virtual void ShortUrl(object sender, EventArgs eventArgs)
  {
    Factory.ActualFactory.GetRequestProcessManager().ProcessRequest(true, false, null, true);
  }
}