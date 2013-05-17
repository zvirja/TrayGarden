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
    public class StandaloneIconService : PlantServiceBase<StandaloneIconPlantBox>
    {

        public StandaloneIconService()
        {
            LuggageName = "StandaloneIconService";
        }


        protected virtual void InitializePlantFromPipeline(IPlant plant)
        {
            InitPlantSIPipeline.Run(plant, LuggageName, CloseComponentClick, ExitGardenClick);
        }

        protected void ExitGardenClick(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        protected void CloseComponentClick(object sender, EventArgs eventArgs)
        {
            var toolStrip = sender as ToolStripItem;
            if (toolStrip == null)
                return;
            var siBox = toolStrip.Tag as StandaloneIconPlantBox;
            if (siBox == null)
                return;
            siBox.IsEnabled = false;
        }

        protected override void PlantOnEnabledChanged(IPlant plant, bool newValue)
        {
            var siBox = GetPlantLuggage(plant);
            if (siBox != null)
                siBox.FixNIVisibility();
        }

        
        
        public override void InitializePlant(IPlant plant)
        {
            base.InitializePlant(plant);
            InitializePlantFromPipeline(plant);
        }
        

       public override void InformDisplayStage()
        {
           base.InformDisplayStage();
            List<IPlant> enabledPlants = HatcherGuide<IGardenbed>.Instance.GetEnabledPlants();
            foreach (IPlant enabledPlant in enabledPlants)
            {
                var siBox = GetPlantLuggage(enabledPlant);
                if (siBox == null)
                    continue;
                siBox.FixNIVisibility();
            }
        }

        public override void InformClosingStage()
        {
            base.InformClosingStage();
            List<IPlant> allPlants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
            foreach (IPlant plant in allPlants)
            {
                var siBox = GetPlantLuggage(plant);
                if(siBox != null)
                    siBox.NotifyIcon.Dispose();
            }
        }

    }
}
