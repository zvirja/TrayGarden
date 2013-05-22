using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.MyAdminConfig.Smorgasbord;

namespace TrayGarden.Services.PlantServices.MyAdminConfig.Core
{
    [UsedImplicitly]
    public class MyAdminConfigService:IService
    {
        public virtual string LuggageName { get; private set; }

        protected virtual void ProvidePlantWithConfig(IPlantEx plantEx)
        {
            var asExpected = plantEx.GetFirstWorkhorseOfType<IGiveMeMyAppConfig>();
            if (asExpected == null)
                return;
            var assemblyLocation = plantEx.Plant.GetType().Assembly.Location;
            System.Configuration.Configuration assemblyConfiguration = null;
            try
            {
                assemblyConfiguration = ConfigurationManager.OpenExeConfiguration(assemblyLocation);
            }
            catch
            {
            }
            if(assemblyConfiguration != null)
                asExpected.SetModuleConfiguration(assemblyConfiguration);
        }

        
        public virtual void InitializePlant(IPlantEx plantEx)
        {
            ProvidePlantWithConfig(plantEx);
        }

        public virtual void InformInitializeStage()
        {
           
        }

        public virtual void InformDisplayStage()
        {
            
        }

        public virtual void InformClosingStage()
        {
            
        }
    }
}
