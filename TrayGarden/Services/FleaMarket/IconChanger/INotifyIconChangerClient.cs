using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace TrayGarden.Services.FleaMarket.IconChanger
{
    public interface INotifyIconChangerClient
    {
        void SetIcon(Icon newIcon, int msTimeout);
        void SetIcon(Icon newIcon);
    }
}