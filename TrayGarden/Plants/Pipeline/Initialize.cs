using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants.Pipeline
{
    [UsedImplicitly]
    public class Initialize
    {

        [UsedImplicitly]
        public virtual void Process(InitializePlantArgs args)
        {
            string id = args.PlantWorkhorse.GetType().FullName;
            ISettingsBox plantSettingsBox = args.RootSettingsBox.GetSubBox(id);
            args.ResolvedPlant.Initialize(args.PlantWorkhorse, id, plantSettingsBox);
        }


    }
}
