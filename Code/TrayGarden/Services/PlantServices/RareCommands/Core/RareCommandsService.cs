using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit;

namespace TrayGarden.Services.PlantServices.RareCommands.Core
{
  [UsedImplicitly]
  public class RareCommandsService : PlantServiceBase<RareCommandsServicePlantBox>
  {
    public RareCommandsService()
      : base("Rare Commands", "RareCommandsService")
    {
      this.ServiceDescription = "This service allows to specify rare commands, which are available only thorough the main window.";
    }

    public override void InitializePlant(IPlantEx plantEx)
    {
      base.InitializePlant(plantEx);
      this.InitializePlantInternal(plantEx);
    }

    protected virtual void InitializePlantInternal(IPlantEx plantEx)
    {
      List<IRareCommand> relatedCommands = InitPlantRareCommands.RunPipelineGetCommands(plantEx);
      if (relatedCommands != null)
      {
        var luggage = new RareCommandsServicePlantBox(relatedCommands);
        plantEx.PutLuggage(this.LuggageName, luggage);
      }
    }
  }
}