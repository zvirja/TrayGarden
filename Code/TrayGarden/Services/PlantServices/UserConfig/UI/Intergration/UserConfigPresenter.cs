using System.Windows;
using System.Windows.Input;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserConfig.UI.Intergration;

public class UserConfigPresenter : ServicePresenterBase<UserConfigService>
{
  private readonly IPipelineRunner _pipelineRunner;

  private readonly IUIManager _uiManager;

  public UserConfigPresenter(IServicesSteward servicesSteward, IPipelineRunner pipelineRunner, IUIManager uiManager)
    : base(servicesSteward)
  {
    _pipelineRunner = pipelineRunner;
    _uiManager = uiManager;
    ServiceName = "Runtime user settings";
    ServiceDescription = "This service allows to configure user settings for plant";
  }

  protected virtual ICommand GetCommand(object plantEx)
  {
    var relayCommand = new RelayCommand(RunServiceForPlant, true);
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
    return new ServiceForPlantActionPerformVM(ServiceName, ServiceDescription, GetCommand(userConfigServicePlantBox));
  }

  protected virtual void RunServiceForPlant(object argument)
  {
    var userConfigServicePlantBox = argument as UserConfigServicePlantBox;
    Assert.IsNotNull(userConfigServicePlantBox, "Wrong argument. Shouldn't be null");

    var args = new GetUCStepPipelineArgs(userConfigServicePlantBox);
    _pipelineRunner.Run(args);
    if (args.Aborted || args.StateConstructInfo.ResultState == null)
    {
      _uiManager.OKMessageBox(
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
