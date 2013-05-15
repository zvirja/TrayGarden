using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core
{
    public class StandaloneIconService:IService
    {

        public string LuggageName { get; set; }

        public StandaloneIconService()
        {
            LuggageName = "StandaloneIconService";
        }
        
        
        
        
        public virtual void InitializePlant(IPlant plant)
        {
            InitializePlantFromPipeline(plant);
            plant.EnabledChanged += PlantOnEnabledChanged;
        }

       

        public void InformInitializeStage()
        {
            
        }

        public virtual void InformDisplayStage()
        {
            List<IPlant> enabledPlants = HatcherGuide<IGardenbed>.Instance.GetEnabledPlants();
            foreach (IPlant enabledPlant in enabledPlants)
            {
                var siBox = GetRelatedSIBox(enabledPlant);
                if (siBox == null)
                    continue;
                siBox.FixNIVisibility();
            }
        }

        public virtual void InformClosingStage()
        {
            List<IPlant> allPlants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
            foreach (IPlant plant in allPlants)
            {
                var siBox = GetRelatedSIBox(plant);
                if(siBox != null)
                    siBox.NotifyIcon.Dispose();
            }
        }

        protected virtual void InitializePlantFromPipeline(IPlant plant)
        {
            InitPlantSIPipeline.Run(plant,LuggageName,CloseComponentClick, ExitGardenClick);
        }

        private void ExitGardenClick(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void CloseComponentClick(object sender, EventArgs eventArgs)
        {
            var toolStrip = sender as ToolStripItem;
            if (toolStrip == null)
                return;
            var siBox = toolStrip.Tag as StandaloneIconPlantBox;
            if (siBox == null)
                return;
            siBox.IsEnabled = false;
        }

        protected virtual StandaloneIconPlantBox GetRelatedSIBox(IPlant plant)
        {
            if (!plant.HasLuggage(LuggageName))
                return null;
            return plant.GetLuggage<StandaloneIconPlantBox>(LuggageName);
        }

        protected virtual void PlantOnEnabledChanged(IPlant plant, bool newValue)
        {
            var siBox = GetRelatedSIBox(plant);
            if(siBox != null)
                siBox.FixNIVisibility();
        }


    }
}
