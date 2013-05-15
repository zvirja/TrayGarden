using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
    public class CreateSIPlantBox
    {
        public virtual void Process(InitPlantSIArgs args)
        {
            args.SIBox = new StandaloneIconPlantBox();
        }
    }
}
