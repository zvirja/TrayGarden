#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit;

#endregion

namespace TrayGarden.Services.PlantServices.RareCommands.Core
{
  [UsedImplicitly]
  public class RareCommandsService : PlantServiceBase<RareCommandsServicePlantBox>
  {
    #region Constructors and Destructors

    public RareCommandsService()
      : base("Rare Commands", "RareCommandsService")
    {
      this.ServiceDescription = "This service allows to specify rare commands, which are available only thorough the main window.";
    }

    #endregion

    #region Public Methods and Operators

    public override void InitializePlant(IPlantEx plantEx)
    {
      base.InitializePlant(plantEx);
      this.InitializePlantInternal(plantEx);
    }

    #endregion

    #region Methods

    protected virtual void InitializePlantInternal(IPlantEx plantEx)
    {
      List<IRareCommand> relatedCommands = InitPlantRareCommands.RunPipelineGetCommands(plantEx);
      if (relatedCommands != null)
      {
        var luggage = new RareCommandsServicePlantBox(relatedCommands);
        plantEx.PutLuggage(this.LuggageName, luggage);
      }
    }

    #endregion
  }
}