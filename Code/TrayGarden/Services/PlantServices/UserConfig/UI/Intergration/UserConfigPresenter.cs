using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserConfig.UI.Intergration
{
  public class UserConfigPresenter : ServicePresenterBase<UserConfigService>
  {
    public UserConfigPresenter()
    {
      this.ServiceName = "Runtime user settings";
      this.ServiceDescription = "This service allows to configure user settings for plant";
    }

    protected virtual ICommand GetCommand(object plantEx)
    {
      var relayCommand = new RelayCommand(this.RunServiceForPlant, true);
      return new CommandProxyForCustomParam(relayCommand, plantEx);
    }

    protected override ServiceForPlantVMBase GetServiceVM(UserConfigService serviceInstance, IPlantEx plantEx)
    {
      UserConfigServicePlantBox userConfigServicePlantBox = serviceInstance.GetPlantLuggage(plantEx);
      if (userConfigServicePlantBox == null)
      {
        return null;
      }
      if (userConfigServicePlantBox.SettingsSteward.DefinedSettings.Count == 0)
      {
        return null;
      }
      return new ServiceForPlantActionPerformVM(this.ServiceName, this.ServiceDescription, this.GetCommand(userConfigServicePlantBox));
    }

    protected virtual void RunServiceForPlant(object argument)
    {
      var userConfigServicePlantBox = argument as UserConfigServicePlantBox;
      Assert.IsNotNull(userConfigServicePlantBox, "Wrong argument. Shouldn't be null");

      var args = new GetUCStepPipelineArgs(userConfigServicePlantBox);
      GetUCStepPipelineRunner.Run(args);
      if (args.Aborted || args.StateConstructInfo.ResultState == null)
      {
        HatcherGuide<IUIManager>.Instance.OKMessageBox(
          "Plant configuration",
          "Plant configuration service wasn't able to resolve next step. Please contact dev",
          MessageBoxImage.Error);
      }
      else
      {
        var nextStep = args.StateConstructInfo.ResultState;
        WindowWithBackVM.GoAheadWithBackIfPossible(nextStep);
      }
    }
  }
}