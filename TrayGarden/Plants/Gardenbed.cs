using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Plants.Pipeline;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Plants
{
    public class Gardenbed : IGardenbed
    {
        protected Dictionary<string, IPlantEx> Plants { get; set; }
        protected ISettingsBox MySettingsBox { get; set; }
        protected ISettingsBox RootPlantsSettingsBox
        {
            get { return MySettingsBox.GetSubBox("Plants"); }
        }
        protected bool Initialized { get; set; }


        public Gardenbed()
        {
            Plants = new Dictionary<string, IPlantEx>();
        }

        [UsedImplicitly]
        public virtual void Initialize([NotNull] List<object> plants)
        {
            Assert.ArgumentNotNull(plants, "plants");
            MySettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("Gargedbed");
            foreach (object plant in plants)
            {
                IPlantEx resolvedPlantEx = ResolveIPlantEx(plant);
                if (resolvedPlantEx != null)
                    Plants.Add(resolvedPlantEx.ID, resolvedPlantEx);
            }
            HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow();
            Initialized = true;
        }

        public virtual List<IPlantEx> GetAllPlants()
        {
            AssertInitialized();
            return Plants.Select(x => x.Value).ToList();
        }

        public virtual List<IPlantEx> GetEnabledPlants()
        {
            AssertInitialized();
            return Plants.Select(x => x.Value).Where(x => x.IsEnabled).ToList();
        }

        protected virtual IPlantEx ResolveIPlantEx(object plant)
        {
            var newPlant = InitializePlantExPipeline.Run(plant, RootPlantsSettingsBox);
            return newPlant;
        }

        protected virtual void AssertInitialized()
        {
            if(!Initialized)
                throw new NonInitializedException();
        }

    }
}
