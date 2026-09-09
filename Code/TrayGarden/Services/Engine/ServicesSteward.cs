using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;

namespace TrayGarden.Services.Engine;

[UsedImplicitly]
public class ServicesSteward : IServicesSteward
{
  private readonly IGardenbed _gardenbed;

  public ServicesSteward(IEnumerable<IService> services, IGardenbed gardenbed)
  {
    _gardenbed = gardenbed;
    Services = services.ToList();
    Initialized = true;
  }

  public List<IService> Services { get; set; }

  private bool Initialized { get; set; }

  public void InformClosingStage()
  {
    AssertInitialized();
    foreach (IService service in Services)
    {
      try
      {
        service.InformClosingStage();
      }
      catch (Exception ex)
      {
        Log.For(this).Error(ex, "Failed to close service {ServiceType}", service.GetType().FullName);
      }
    }
  }

  public void InformDisplayStage()
  {
    AssertInitialized();
    foreach (IService service in Services)
    {
      try
      {
        if (service.IsActuallyEnabled)
        {
          service.InformDisplayStage();
        }
        else
        {
          Log.For(this).Debug("service {ServiceName} skipped display initialize stage. It's disabled", service.ServiceName);
        }
      }
      catch (Exception ex)
      {
        Log.For(this).Error(ex, "Failed to display service {ServiceType}", service.GetType().FullName);
      }
    }
  }

  public void InformInitializeStage()
  {
    AssertInitialized();

    foreach (IService service in Services)
    {
      try
      {
        if (service.IsActuallyEnabled)
        {
          service.InformInitializeStage();
        }
        else
        {
          Log.For(this).Debug("service {ServiceName} skipped initialize stage. It's disabled", service.ServiceName);
        }
      }
      catch (Exception ex)
      {
        Log.For(this).Error(ex, "Failed to init service {ServiceType}", service.GetType().FullName);
      }
    }
    var plants = _gardenbed.GetAllPlants();
    foreach (IPlantEx plant in plants)
    {
      AquaintPlantWithServices(plant);
    }
  }

  private void AquaintPlantWithServices(IPlantEx plantEx)
  {
    foreach (IService service in Services)
    {
      try
      {
        service.InitializePlant(plantEx);
      }
      catch (Exception ex)
      {
        Log.For(this).Error(
          ex,
          "Failed to init plant '{PlantType}' with service {ServiceType}",
          plantEx.Plant.GetType().FullName,
          service.GetType().FullName);
      }
    }
  }

  private void AssertInitialized()
  {
    if (!Initialized)
    {
      throw new NonInitializedException();
    }
  }
}