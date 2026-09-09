using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.RareCommands.UI;

public class RareCommandsPresenter(IServicesSteward servicesSteward, IUIManager uiManager)
  : IPipelineProcessor<ResolveSinglePlantVMPipelineArgs>
{
  public virtual void Process(ResolveSinglePlantVMPipelineArgs args)
  {
    var service = (RareCommandsService)servicesSteward.Services.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(RareCommandsService)));
    if (service == null)
    {
      return;
    }
    if (!service.IsActuallyEnabled)
    {
      return;
    }
    if (!service.IsAvailableForPlant(args.PlantEx))
    {
      return;
    }
    List<ServiceForPlantVMBase> resultVMs = GetActionsVM(service, args.PlantEx);
    if (resultVMs == null)
    {
      return;
    }
    foreach (ServiceForPlantVMBase vmBase in resultVMs)
    {
      args.PlantVM.ServicesVM.Add(vmBase);
    }
  }

  protected virtual List<ServiceForPlantVMBase> GetActionsVM(RareCommandsService serviceInstance, IPlantEx plantEx)
  {
    RareCommandsServicePlantBox luggage = serviceInstance.GetPlantLuggage(plantEx);
    List<IRareCommand> settings = luggage.RareCommands;
    if (settings == null)
    {
      return null;
    }
    if (settings.Count == 0)
    {
      return null;
    }
    var result = new List<ServiceForPlantVMBase>();
    foreach (IRareCommand rareCommand in settings)
    {
      result.Add(GetRareCommandActionVM(rareCommand));
    }
    return result;
  }

  protected virtual ICommand GetCommandWrapper(IRareCommand rareCommand)
  {
    return new RareCommandWrapper(uiManager, rareCommand);
  }

  protected virtual ServiceForPlantVMBase GetRareCommandActionVM(IRareCommand rareCommand)
  {
    return new ServiceForPlantActionPerformVM(uiManager, rareCommand.Title, rareCommand.Description, GetCommandWrapper(rareCommand));
  }
}
