using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;
using TrayGarden.Helpers;

namespace TrayGarden.Services.Engine
{
  [UsedImplicitly]
  public class ServicesSteward : IServicesSteward
  {
    protected bool Initialized { get; set; }

    public List<IService> Services { get; set; }

    [UsedImplicitly]
    public void Initialize([NotNull] List<IService> services)
    {
      Assert.ArgumentNotNull(services, "services");
      Services = services;
      Initialized = true;
    }

    public virtual void InformInitializeStage()
    {
      AssertInitialized();
      using (new BulkSettingsUpdate())
      {
        foreach (IService service in Services)
        {
          try
          {
            if (service.IsActuallyEnabled)
              service.InformInitializeStage();
            else
              Log.Debug("service {0} skipped initialize stage. It's disabled".FormatWith(service.ServiceName), this);
          }
          catch (Exception ex)
          {
            Log.Error("Failed to init service {0}".FormatWith(service.GetType().FullName), ex, this);
          }
        }
        var plants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
        foreach (IPlantEx plant in plants)
          AquaintPlantWithServices(plant);
      }
    }

    public virtual void InformDisplayStage()
    {
      AssertInitialized();
      using (new BulkSettingsUpdate())
      {
        foreach (IService service in Services)
          try
          {
            if (service.IsActuallyEnabled)
              service.InformDisplayStage();
            else
              Log.Debug("service {0} skipped display initialize stage. It's disabled".FormatWith(service.ServiceName), this);
          }
          catch (Exception ex)
          {
            Log.Error("Failed to display service {0}".FormatWith(service.GetType().FullName), ex, this);

          }
      }
    }

    public virtual void InformClosingStage()
    {
      AssertInitialized();
      using (new BulkSettingsUpdate())
      {
        foreach (IService service in Services)
          try
          {
            service.InformClosingStage();
          }
          catch (Exception ex)
          {
            Log.Error("Failed to close service {0}".FormatWith(service.GetType().FullName), ex, this);

          }
      }
    }

    protected virtual void AquaintPlantWithServices(IPlantEx plantEx)
    {
      foreach (IService service in Services)
      {
        try
        {
          service.InitializePlant(plantEx);
        }
        catch (Exception ex)
        {
          Log.Error("Failed to init plant '{0}' with service {1}".FormatWith(plantEx.Plant.GetType().FullName, service.GetType().FullName), ex, this);
        }
      }
    }

    protected virtual void AssertInitialized()
    {
      if (!Initialized)
        throw new NonInitializedException();
    }

  }
}
