using System;
using System.Drawing;
using System.Windows.Forms;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Smorgasbord
{
    public interface IExtendsGlobalMenu
    {
        bool GetMenuStripItemData(out string text, out Icon icon, out EventHandler clickHandler);
    }
}
