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

        protected virtual void ProvidePlantWithConfig(IPlantInternal plantInternal)
        {
            var asExpected = plantInternal.Workhorse as IGiveMeMyAppConfig;
            if (asExpected == null)
                return;
            var assemblyLocation = plantInternal.Workhorse.GetType().Assembly.Location;
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

        
        public virtual void InitializePlant(IPlantInternal plantInternal)
        {
            ProvidePlantWithConfig(plantInternal);
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
