using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Reception.Services.StandaloneIcon;

/// <summary>
/// This interface allows to modify the notify icon, created by IStandaloneIcon interface.
/// Pay attention that object may implement IStandaloneIcon to get it work.
/// </summary>
public interface INeedToModifyIcon
{
  void StoreIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient);
}