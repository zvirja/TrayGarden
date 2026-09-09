using System.Linq;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;

[UsedImplicitly]
public abstract class ServicePresenterBase<TServiceType> : IPipelineProcessor<ResolveSinglePlantVMPipelineArgs>
  where TServiceType : IService
{
  protected ServicePresenterBase(IServicesSteward servicesSteward, IUIManager uiManager)
  {
    ServicesSteward = servicesSteward;
    UIManager = uiManager;
    ServiceName = typeof(TServiceType).Name;
    ServiceDescription = "<this service doesn't provide description>";
  }

  public string ServiceDescription { get; set; }

  public string ServiceName { get; set; }

  protected IServicesSteward ServicesSteward { get; }

  protected IUIManager UIManager { get; }

  [UsedImplicitly]
  public virtual void Process(ResolveSinglePlantVMPipelineArgs args)
  {
    var serviceInstance =
      (TServiceType)(ServicesSteward.Services.FirstOrDefault(x => x.GetType() == typeof(TServiceType)));
    if (serviceInstance == null)
    {
      Log.For(this).Warning("Service of type '{ServiceType}' wasn't found", typeof(TServiceType));
      return;
    }
    if (!serviceInstance.IsActuallyEnabled)
    {
      return;
    }
    bool isAvailableForPlant = serviceInstance.IsAvailableForPlant(args.PlantEx);
    if (!isAvailableForPlant)
    {
      return;
    }
    ServiceForPlantVMBase serviceForPlantVMBase = GetServiceVM(serviceInstance, args.PlantEx);
    if (serviceForPlantVMBase != null)
    {
      args.PlantVM.ServicesVM.Add(serviceForPlantVMBase);
    }
  }

  protected abstract ServiceForPlantVMBase GetServiceVM(TServiceType serviceInstance, IPlantEx plantEx);
}
