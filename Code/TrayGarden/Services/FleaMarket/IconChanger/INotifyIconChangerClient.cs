using System.Drawing;

namespace TrayGarden.Services.FleaMarket.IconChanger
{
  public interface INotifyIconChangerClient
  {
    void NotifySuccess(int msTimeout = 0);
    
    void NotifyFailed(int msTimeout = 0);

    void SetIcon(Icon newIcon, int msTimeout);

    void SetIcon(Icon newIcon);
  }
}