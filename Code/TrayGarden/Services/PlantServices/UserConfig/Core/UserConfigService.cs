using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

[UsedImplicitly]
public class UserConfigService : PlantServiceBase<UserConfigServicePlantBox>
{
  private readonly IPipelineRunner _pipelineRunner;

  public UserConfigService(IRuntimeSettingsManager runtimeSettingsManager, IPipelineRunner pipelineRunner)
    : base(runtimeSettingsManager, "User Config", "UserConfigService")
  {
    _pipelineRunner = pipelineRunner;
    ServiceDescription = "Service provides plant with user-configurable settings. These settings may be configured through UI.";
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    base.InitializePlant(plantEx);
    InitializePlantInternal(plantEx);
  }

  protected virtual void InitializePlantInternal(IPlantEx plantEx)
  {
    _pipelineRunner.Run(new InitPlantUCPipelineArg(LuggageName, plantEx));
  }
}
