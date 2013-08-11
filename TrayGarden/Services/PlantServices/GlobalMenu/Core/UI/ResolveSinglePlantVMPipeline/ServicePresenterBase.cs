﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core;
using TrayGarden.TypesHatcher;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
    [UsedImplicitly]
    public abstract class ServicePresenterBase<TServiceType> where TServiceType:IService
    {
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }

        public ServicePresenterBase()
        {
            ServiceName = typeof (TServiceType).Name;
            ServiceDescription = "<this service doesn't provide description>";
        }

        [UsedImplicitly]
        public virtual void Process(ResolveSinglePlantVMPipelineArgs args)
        {
            var serviceInstance =
                (TServiceType)(HatcherGuide<IServicesSteward>.Instance.Services.FirstOrDefault(
                    x => x.GetType() == typeof(TServiceType)));
            if (serviceInstance == null)
            {
                Log.Warn("Service of type '{0}' wasn't found".FormatWith(typeof(TServiceType)), this);
                return;
            }
            if (!serviceInstance.IsActuallyEnabled)
                return;
            bool isAvailableForPlant = serviceInstance.IsAvailableForPlant(args.PlantEx);
            if (!isAvailableForPlant)
                return;
            ServiceForPlantVMBase serviceForPlantVMBase = GetServiceVM(serviceInstance, args.PlantEx);
            if (serviceForPlantVMBase != null)
                args.PlantVM.ServicesVM.Add(serviceForPlantVMBase);
        }

        protected abstract ServiceForPlantVMBase GetServiceVM(TServiceType serviceInstance, IPlantEx plantEx);
     
    }
}
