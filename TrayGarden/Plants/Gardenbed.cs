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
        protected Dictionary<string, IPlant> Plants { get; set; }
        protected ISettingsBox MySettingsBox { get; set; }
        protected ISettingsBox RootPlantsSettingsBox
        {
            get { return MySettingsBox.GetSubBox("Plants"); }
        }


        public Gardenbed()
        {
            Plants = new Dictionary<string, IPlant>();
        }

        [UsedImplicitly]
        public virtual void Initialize([NotNull] List<object> workhorses)
        {
            if (workhorses == null) throw new ArgumentNullException("workhorses");
            MySettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("Gargedbed");
            foreach (object workhorse in workhorses)
            {
                IPlant resolvedPlant = ResolveIPlant(workhorse);
                if (resolvedPlant != null)
                    Plants.Add(resolvedPlant.ID, resolvedPlant);
            }
            HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow();
        }

        public virtual List<IPlant> GetAllPlants()
        {
            return Plants.Select(x => x.Value).ToList();
        }

        public virtual List<IPlant> GetEnabledPlants()
        {
            return Plants.Select(x => x.Value).Where(x => x.IsEnabled).ToList();
        }

        protected virtual IPlant ResolveIPlant(object workhorse)
        {
            var newPlant = InitializePlantPipeline.Run(workhorse, RootPlantsSettingsBox);
            return newPlant;
        }

    }
}
