﻿using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

[UsedImplicitly]
public class UserConfigService : PlantServiceBase<UserConfigServicePlantBox>
{
  public UserConfigService()
    : base("User Config", "UserConfigService")
  {
    ServiceDescription = "Service provides plant with user-configurable settings. These settings may be configured through UI.";
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    base.InitializePlant(plantEx);
    InitializePlantInternal(plantEx);
  }

  protected virtual void InitializePlantInternal(IPlantEx plantEx)
  {
    InitPlantUCPipeline.Run(LuggageName, plantEx);
  }
}