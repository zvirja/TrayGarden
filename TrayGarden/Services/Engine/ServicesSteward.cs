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
                try
                {
                    service.InformInitializeStage();
                }
                catch
                {
                    //TODO IMPLEMENT LOGGING HERE
                }
            var plants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
            foreach (IPlantEx plant in plants)
                    AquaintPlantWithServices(plant);
        }

        public virtual void InformDisplayStage()
        {
            AssertInitialized();
            foreach (IService service in Services)
                try
                {
                    service.InformDisplayStage();
                }
                catch
                {
                    //TODO IMPLEMENT LOGGING HERE
                }
        }

        public virtual void InformClosingStage()
        {
            AssertInitialized();
            foreach (IService service in Services)
                try
                {
                    service.InformClosingStage();
                }
                catch (Exception e)
                {
                    //TODO IMPLEMENT LOGGING HERE
                }
        }

        protected virtual void AquaintPlantWithServices(IPlantEx plantEx)
        {
            foreach (IService service in Services)
            {
                try
                {
                    service.InitializePlant(plantEx);
                }
                catch (Exception e)
                {
                    //TODO IMPLEMENT LOGGING HERE
                }
            }
        }

        protected virtual void AssertInitialized()
        {
            if (!Initialized)
                throw new NonInitializedException();
        }

    }
}
