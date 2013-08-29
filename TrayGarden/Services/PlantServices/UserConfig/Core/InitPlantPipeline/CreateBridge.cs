using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class CreateBridge
  {
    public IObjectFactory BridgeFactory { get; set; }
    public IUserSettingChangedStrategy DefaultNotifyStrategy { get; set; }

    [UsedImplicitly]
    public virtual void Process(InitPlantUCPipelineArg args)
    {
      var bridge = CreateNewBridge();
      bridge.Initialize(args.Settings, GetDefaultStrategy());
      args.Bridge = bridge;
    }

    protected virtual IUserSettingsBridgeMaster CreateNewBridge()
    {
      if (BridgeFactory != null)
      {
        var bridge = BridgeFactory.GetPurelyNewObject() as IUserSettingsBridgeMaster;
        if (bridge != null)
          return bridge;
      }
      return new UserSettingsBridge();
    }

    protected virtual IUserSettingChangedStrategy GetDefaultStrategy()
    {
      return DefaultNotifyStrategy ?? new ImpatientNotifyingStrategy();
    }
  }
}
