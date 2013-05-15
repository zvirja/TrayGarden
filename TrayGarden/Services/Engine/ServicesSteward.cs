using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Plants;

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


        public virtual void AquaintPlantWithServices(IPlant plant)
        {
            AssertInitialized();
            foreach (IService service in Services)
            {
                service.InitializePlant(plant);
            }
        }

        public virtual void InformInitializeStage()
        {
            AssertInitialized();
            foreach (IService service in Services)
                service.InformInitializeStage();
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

        protected virtual void AssertInitialized()
        {
            if (!Initialized)
                throw new NonInitializedException();
        }

    }
}
