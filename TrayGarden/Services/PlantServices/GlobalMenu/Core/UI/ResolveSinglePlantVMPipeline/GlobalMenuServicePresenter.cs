using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
    [UsedImplicitly]
    public class GlobalMenuServicePresenter:ServicePresenterBase<GlobalMenuService>
    {
        public GlobalMenuServicePresenter()
        {
            ServiceName = "Embedding to global menu";
        }

        protected override ServiceForPlantVMBase GetServiceVM(GlobalMenuService serviceInstance, IPlantEx plantEx)
        {
            return new ServiceForPlantWithEnablingVM();
        }
    }
}
