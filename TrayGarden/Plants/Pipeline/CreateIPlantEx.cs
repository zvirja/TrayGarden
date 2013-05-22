using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.Plants.Pipeline
{
    [UsedImplicitly]
    public class CreateIPlantEx
    {
        [UsedImplicitly]
        public virtual void Process(InitializePlantArgs args)
        {
            var newPlant = new PlantEx();
            args.ResolvedPlantEx = newPlant;
        }

    }
}
