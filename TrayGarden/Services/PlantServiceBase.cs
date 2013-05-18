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


        protected TPlantLuggageType GetPlantLuggage(IPlantEx plantEx)
        {
            if (!plantEx.HasLuggage(LuggageName))
                return null;
            return plantEx.GetLuggage<TPlantLuggageType>(LuggageName);
        }

        protected virtual void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
        {

        }

        public virtual void InitializePlant(IPlantEx plantEx)
        {
            plantEx.EnabledChanged += PlantOnEnabledChanged;
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
