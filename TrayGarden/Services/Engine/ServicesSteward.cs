using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.Engine
{
    [UsedImplicitly]
    public class ServicesSteward : IServicesSteward
    {
        protected bool Initialized { get; set; }

        public List<IService> Services { get; set; }

        [UsedImplicitly]
        public void Initialize([NotNull] List<IService> services)
        {
            if (services == null) throw new ArgumentNullException("services");
            Services = services;
            Initialized = true;
        }

        public virtual void InformInitializeStage()
        {
            AssertInitialized();
            foreach (IService service in Services)
                service.InformInitializeStage();
            var plants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
            foreach (IPlant plant in plants)
                AquaintPlantWithServices(plant);
        }

        public virtual void InformDisplayStage()
        {
            AssertInitialized();
            foreach (IService service in Services)
                service.InformDisplayStage();
        }

        public virtual void InformClosingStage()
        {
            AssertInitialized();
            foreach (IService service in Services)
                service.InformClosingStage();
        }

        protected virtual void AquaintPlantWithServices(IPlant plant)
        {
            foreach (IService service in Services)
            {
                service.InitializePlant(plant);
            }
        }

        protected virtual void AssertInitialized()
        {
            if (!Initialized)
                throw new NonInitializedException();
        }

    }
}
