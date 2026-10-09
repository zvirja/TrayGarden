using System;
using System.Threading;
using System.Threading.Tasks;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core;

public sealed class PlantNotifier(IToastGate toastGate, UserNotificationsServicePlantBox plantBox) : IUserNotifier
{
  public Task<string> ShowAsync(Toast toast, CancellationToken cancellationToken = default)
  {
    if (!plantBox.IsEnabled || !plantBox.RelatedPlantEx.IsEnabled)
    {
      return Task.FromResult<string>(null);
    }

    return toastGate.ShowAsync(toast, plantBox.RelatedPlantEx.Plant.HumanSupportingName, cancellationToken);
  }
}
