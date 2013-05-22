using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Hallway;

namespace TrayGarden.Plants.Pipeline
{
    [UsedImplicitly]
    public class ResolveIPlant
    {
        [UsedImplicitly]
        public virtual void Process(InitializePlantArgs args)
        {
            var iPlant = args.PlantObject as IPlant;
            if (iPlant != null)
            {
                args.IPlantObject = iPlant;
            }
            else
            {
                args.Abort();
            }
        }

    }
}
