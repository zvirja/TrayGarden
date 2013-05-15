using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace TrayGarden.Services.FleaMarket.IconChanger
{
    public interface INotifyIconChanger
    {
        void SetIcon(Icon newIcon, int msTimeout);
        void SetIcon(Icon newIcon);
    }
}