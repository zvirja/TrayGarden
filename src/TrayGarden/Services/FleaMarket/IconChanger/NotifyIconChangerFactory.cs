using System;

using Microsoft.Extensions.DependencyInjection;

namespace TrayGarden.Services.FleaMarket.IconChanger;

public class NotifyIconChangerFactory(IServiceProvider serviceProvider) : INotifyIconChangerFactory
{
  public INotifyIconChangerMaster Create()
  {
    return serviceProvider.GetRequiredService<INotifyIconChangerMaster>();
  }
}
