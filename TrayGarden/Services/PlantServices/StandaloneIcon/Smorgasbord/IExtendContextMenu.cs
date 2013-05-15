using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Smorgasbord
{
    public interface IExtendContextMenu
    {
        List<ToolStripItem> GetStripsToAdd();
    }
}
