#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TrayGarden.Helpers;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting
{
  public class MenuEntriesAppender : IMenuEntriesAppender
  {
    #region Constructors and Destructors

    public MenuEntriesAppender()
    {
      this.OutputItems = new List<ToolStripMenuItem>();
    }

    #endregion

    #region Public Properties

    public List<ToolStripMenuItem> OutputItems { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual void AppentMenuStripItem(string text, Icon icon, EventHandler clickHandler)
    {
      if (text.IsNullOrEmpty() || icon == null || clickHandler == null)
      {
        return;
      }
      EventHandler asyncClickHandler = (sender, args) => Task.Factory.StartNew(() => clickHandler(sender, args));
      var menuItem = new ToolStripMenuItem(text, icon.ToBitmap(), asyncClickHandler);
      this.OutputItems.Add(menuItem);
    }

    #endregion
  }
}