using System.Collections.Generic;

using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit;

namespace TrayGarden.Services.PlantServices.RareCommands.Core;

[UsedImplicitly]
public class RareCommandsService : PlantServiceBase<RareCommandsServicePlantBox>
{
  private readonly IPipelineRunner _pipelineRunner;

  public RareCommandsService(IRuntimeSettingsManager runtimeSettingsManager, IPipelineRunner pipelineRunner)
    : base(runtimeSettingsManager, "Rare Commands", "RareCommandsService")
  {
    _pipelineRunner = pipelineRunner;
    ServiceDescription = "This service allows to specify rare commands, which are available only thorough the main window.";
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    base.InitializePlant(plantEx);
    InitializePlantInternal(plantEx);
  }

  protected virtual void InitializePlantInternal(IPlantEx plantEx)
  {
    var pipelineArgs = new InitPlantRareCommandsArgs(plantEx);
    _pipelineRunner.Run(pipelineArgs);
    List<IRareCommand> relatedCommands = pipelineArgs.CollectedCommands;
    if (relatedCommands != null)
    {
      var luggage = new RareCommandsServicePlantBox(relatedCommands);
      plantEx.PutLuggage(LuggageName, luggage);
    }
  }
}
