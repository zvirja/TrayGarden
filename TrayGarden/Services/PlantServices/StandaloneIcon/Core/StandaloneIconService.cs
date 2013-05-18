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


        protected virtual void InitializePlantFromPipeline(IPlantEx plantEx)
        {
            InitPlantSIPipeline.Run(plantEx, LuggageName, CloseComponentClick, ExitGardenClick);
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

        protected override void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
        {
            var siBox = GetPlantLuggage(plantEx);
            if (siBox != null)
                siBox.FixNIVisibility();
        }

        
        
        public override void InitializePlant(IPlantEx plantEx)
        {
            base.InitializePlant(plantEx);
            InitializePlantFromPipeline(plantEx);
        }
        

       public override void InformDisplayStage()
        {
           base.InformDisplayStage();
            List<IPlantEx> enabledPlants = HatcherGuide<IGardenbed>.Instance.GetEnabledPlants();
            foreach (IPlantEx enabledPlant in enabledPlants)
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
            List<IPlantEx> allPlants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
            foreach (IPlantEx plant in allPlants)
            {
                var siBox = GetPlantLuggage(plant);
                if(siBox != null)
                    siBox.NotifyIcon.Dispose();
            }
        }

    }
}
