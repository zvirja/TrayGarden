using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Smorgasbord
{
    public interface INeedToModifyIcon
    {
        void SetIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient);
    }
}
