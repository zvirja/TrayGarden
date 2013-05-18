using System;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.GlobalMenu.Smorgasbord;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class CreateContextMenuStrip
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantGMArgs args)
        {
            var asExpected = args.PlantEx.Workhorse as IExtendsGlobalMenu;
            if (asExpected == null)
            {
                args.Abort();
                return;
            }
            var menuItem = GetStripItem(asExpected);
            if (menuItem == null)
            {
                args.Abort();
                return;
            }
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
                var menuItem = new ToolStripMenuItem(title, icon.ToBitmap(), clickHandler);
                return menuItem;
            }
            return null;
        }
    }
}
