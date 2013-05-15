using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace TrayGarden.Services.FleaMarket.IconChanger
{
    public interface INotifyIconChangerMaster
    {
        int DefaultDelayMsec { get; set; }
        void Initialize([NotNull] NotifyIcon operableNIcon);
        void SetIcon(Icon newIcon, int msTimeout);
        void SetIcon(Icon newIcon);
    }
}