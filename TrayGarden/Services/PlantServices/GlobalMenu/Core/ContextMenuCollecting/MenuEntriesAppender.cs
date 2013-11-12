using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using TrayGarden.Helpers;
using TrayGarden.Reception.Services;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting
{
  public class MenuEntriesAppender : IMenuEntriesAppender
  {
    public List<ToolStripMenuItem> OutputItems { get; set; }

    public MenuEntriesAppender()
    {
      OutputItems = new List<ToolStripMenuItem>();
    }

    public virtual void AppentMenuStripItem(string text, Icon icon, EventHandler clickHandler)
    {
      if (text.IsNullOrEmpty() || icon == null || clickHandler == null)
        return;
      EventHandler asyncClickHandler = (sender, args) => Task.Factory.StartNew(() => clickHandler(sender, args));
      var menuItem = new ToolStripMenuItem(text, icon.ToBitmap(), asyncClickHandler);
      OutputItems.Add(menuItem);
    }
  }
}