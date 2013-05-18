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


        protected TPlantLuggageType GetPlantLuggage(IPlantInternal plantInternal)
        {
            if (!plantInternal.HasLuggage(LuggageName))
                return null;
            return plantInternal.GetLuggage<TPlantLuggageType>(LuggageName);
        }

        protected virtual void PlantOnEnabledChanged(IPlantInternal plantInternal, bool newValue)
        {

        }

        public virtual void InitializePlant(IPlantInternal plantInternal)
        {
            plantInternal.EnabledChanged += PlantOnEnabledChanged;
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
