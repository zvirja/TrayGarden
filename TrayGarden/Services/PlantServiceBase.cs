using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Plants;

namespace TrayGarden.Services
{
    public abstract class PlantServiceBase<TPlantLuggageType>:IService where TPlantLuggageType : class
    {

        public string LuggageName { get; set; }


        protected TPlantLuggageType GetPlantLuggage(IPlant plant)
        {
            if (!plant.HasLuggage(LuggageName))
                return null;
            return plant.GetLuggage<TPlantLuggageType>(LuggageName);
        }

        protected virtual void PlantOnEnabledChanged(IPlant plant, bool newValue)
        {

        }

        public virtual void InitializePlant(IPlant plant)
        {
            plant.EnabledChanged += PlantOnEnabledChanged;
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
