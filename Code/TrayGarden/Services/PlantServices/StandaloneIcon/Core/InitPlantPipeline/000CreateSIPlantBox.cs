using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class CreateSIPlantBox
  {
    [UsedImplicitly]
    public virtual void Process(InitPlantSIArgs args)
    {
      args.SIBox = new StandaloneIconPlantBox();
    }
  }
}
