using System;

using Microsoft.Extensions.DependencyInjection;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI;

public class NotificationWindowFactory(IServiceProvider serviceProvider) : INotificationWindowFactory
{
  public INotificationWindow Create()
  {
    return serviceProvider.GetRequiredService<INotificationWindow>();
  }
}
