using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Reception.Services;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class CreateContextMenuStrip
  {
    [UsedImplicitly]
    public virtual void Process(InitPlantGMArgs args)
    {
      var asExpected = args.PlantEx.GetFirstWorkhorseOfType<IExtendsGlobalMenu>();
      if (asExpected == null)
      {
        return;
      }
      var menuItem = GetStripItem(asExpected);
      if (menuItem == null)
      {
        return;
      }
      args.IsPlantInUse = true;
      args.GMBox.ToolStripMenuItem = menuItem;
    }

    protected virtual ToolStripMenuItem GetStripItem(IExtendsGlobalMenu instance)
    {
      string title;
      Icon icon;
      EventHandler clickHandler;
      if (instance.GetMenuStripItemData(out title, out icon, out clickHandler))
      {
        if (title.IsNullOrEmpty() || icon == null || clickHandler == null)
          return null;
        EventHandler asyncClickHandler = (sender, args) => Task.Factory.StartNew(() => clickHandler(sender, args));
        var menuItem = new ToolStripMenuItem(title, icon.ToBitmap(), asyncClickHandler);
        return menuItem;
      }
      return null;
    }
  }
}
