#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
  [UsedImplicitly]
  public class UserConfigService : PlantServiceBase<UserConfigServicePlantBox>
  {
    #region Constructors and Destructors

    public UserConfigService()
      : base("User Config", "UserConfigService")
    {
      this.ServiceDescription = "Service provides plant with user-configurable settings. These settings may be configured through UI.";
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
      InitPlantUCPipeline.Run(this.LuggageName, plantEx);
    }

    #endregion
  }
}