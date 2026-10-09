using TrayGarden.Services.PlantServices.UserNotifications.Core;

namespace TrayGarden.Reception.Services;

/// <summary>
/// This service allows plant to show Windows toast notifications.
/// </summary>
public interface IGetPowerOfUserNotifications
{
  void StoreNotifier(IUserNotifier notifier);
}
