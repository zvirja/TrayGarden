using System;
using System.Drawing;
using System.Windows.Forms;
using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Smorgasbord
{
    public interface IChangesGlobalIcon
    {
        void StoreGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient);
    }
}
