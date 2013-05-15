using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Smorgasbord
{
    public interface IStandaloneIcon
    {
        bool GetIconInfo(out string title, out Icon icon, out MouseEventHandler iconClickHandler);
    }
}
