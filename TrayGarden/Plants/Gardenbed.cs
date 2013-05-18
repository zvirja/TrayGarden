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
        protected Dictionary<string, IPlantInternal> Plants { get; set; }
        protected ISettingsBox MySettingsBox { get; set; }
        protected ISettingsBox RootPlantsSettingsBox
        {
            get { return MySettingsBox.GetSubBox("Plants"); }
        }


        public Gardenbed()
        {
            Plants = new Dictionary<string, IPlantInternal>();
        }

        [UsedImplicitly]
        public virtual void Initialize([NotNull] List<object> workhorses)
        {
            if (workhorses == null) throw new ArgumentNullException("workhorses");
            MySettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("Gargedbed");
            foreach (object workhorse in workhorses)
            {
                IPlantInternal resolvedPlantInternal = ResolveIPlant(workhorse);
                if (resolvedPlantInternal != null)
                    Plants.Add(resolvedPlantInternal.ID, resolvedPlantInternal);
            }
            HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow();
        }

        public virtual List<IPlantInternal> GetAllPlants()
        {
            return Plants.Select(x => x.Value).ToList();
        }

        public virtual List<IPlantInternal> GetEnabledPlants()
        {
            return Plants.Select(x => x.Value).Where(x => x.IsEnabled).ToList();
        }

        protected virtual IPlantInternal ResolveIPlant(object workhorse)
        {
            var newPlant = InitializePlantPipeline.Run(workhorse, RootPlantsSettingsBox);
            return newPlant;
        }

    }
}
