#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using TrayGarden.Plants;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.RareCommands.UI
{
  public class RareCommandsPresenter
  {
    #region Public Methods and Operators

    public virtual void Process(ResolveSinglePlantVMPipelineArgs args)
    {
      var service = (RareCommandsService)HatcherGuide<IServicesSteward>.Instance.Services.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(RareCommandsService)));
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
      List<ServiceForPlantVMBase> resultVMs = this.GetActionsVM(service, args.PlantEx);
      if (resultVMs == null)
      {
        return;
      }
      foreach (ServiceForPlantVMBase vmBase in resultVMs)
      {
        args.PlantVM.ServicesVM.Add(vmBase);
      }
    }

    #endregion

    #region Methods

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
        result.Add(this.GetRareCommandActionVM(rareCommand));
      }
      return result;
    }

    protected virtual ICommand GetCommandWrapper(IRareCommand rareCommand)
    {
      return new RareCommandWrapper(rareCommand);
    }

    protected virtual ServiceForPlantVMBase GetRareCommandActionVM(IRareCommand rareCommand)
    {
      return new ServiceForPlantActionPerformVM(rareCommand.Title, rareCommand.Description, this.GetCommandWrapper(rareCommand));
    }

    #endregion
  }
}