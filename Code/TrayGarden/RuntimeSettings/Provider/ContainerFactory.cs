using System;

using Microsoft.Extensions.DependencyInjection;

namespace TrayGarden.RuntimeSettings.Provider;

public class ContainerFactory(IServiceProvider serviceProvider) : IContainerFactory
{
  public IContainer Create()
  {
    return serviceProvider.GetRequiredService<IContainer>();
  }
}
