using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.UserConfig.Core;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
    [UsedImplicitly]
    public class UserSettingServicePresenter : ServicePresenterBase<UserConfigService>
    {
        protected override ServiceForPlantVMBase GetServiceVM(UserConfigService serviceInstance, IPlantEx plantEx)
        {
            throw new NotImplementedException();
        }
    }
}
