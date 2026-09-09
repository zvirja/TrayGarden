using System;

using Microsoft.Extensions.DependencyInjection;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

public class DynamicStateWatcherFactory(IServiceProvider serviceProvider) : IDynamicStateWatcherFactory
{
  public IDynamicStateWatcher Create()
  {
    return serviceProvider.GetRequiredService<IDynamicStateWatcher>();
  }
}
