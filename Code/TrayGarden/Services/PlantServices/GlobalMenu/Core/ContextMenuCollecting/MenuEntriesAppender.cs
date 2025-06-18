using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

public class MenuEntriesAppender : IMenuEntriesAppender
{
  public MenuEntriesAppender()
  {
    this.OutputItems = new List<ExtendedToolStripMenuItem>();
  }

  public List<ExtendedToolStripMenuItem> OutputItems { get; set; }

  public virtual void AppentMenuStripItem(string text, Icon icon, EventHandler clickHandler, IDynamicStateProvider dynamicStateProvider = null)
  {
    if (text.IsNullOrEmpty() || icon == null || clickHandler == null)
    {
      return;
    }
    EventHandler asyncClickHandler = (sender, args) => Task.Factory.StartNew(() => clickHandler(sender, args));
    var menuItem = new ExtendedToolStripMenuItem(text, icon.ToBitmap(), asyncClickHandler) { DymamicStateProvider = dynamicStateProvider };
    this.OutputItems.Add(menuItem);
  }
}