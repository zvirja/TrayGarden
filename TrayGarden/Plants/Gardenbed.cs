using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TrayGarden.Configuration;
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


        public Gardenbed()
        {
            Plants = new Dictionary<string, IPlantEx>();
        }

        [UsedImplicitly]
        public virtual void Initialize([NotNull] List<object> workhorses)
        {
            if (workhorses == null) throw new ArgumentNullException("workhorses");
            MySettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("Gargedbed");
            foreach (object workhorse in workhorses)
            {
                IPlantEx resolvedPlantEx = ResolveIPlant(workhorse);
                if (resolvedPlantEx != null)
                    Plants.Add(resolvedPlantEx.ID, resolvedPlantEx);
            }
            HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow();
        }

        public virtual List<IPlantEx> GetAllPlants()
        {
            return Plants.Select(x => x.Value).ToList();
        }

        public virtual List<IPlantEx> GetEnabledPlants()
        {
            return Plants.Select(x => x.Value).Where(x => x.IsEnabled).ToList();
        }

        protected virtual IPlantEx ResolveIPlant(object workhorse)
        {
            var newPlant = InitializePlantPipeline.Run(workhorse, RootPlantsSettingsBox);
            return newPlant;
        }

    }
}
