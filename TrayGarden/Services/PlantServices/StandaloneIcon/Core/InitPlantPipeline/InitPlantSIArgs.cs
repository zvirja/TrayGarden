using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
    public class InitPlantSIArgs:PipelineArgs
    {
        public string LuggageName { get; set; }
        public EventHandler CloseComponentClick { get; set; }
        public EventHandler ExitGardenClick { get; set; }
        public IPlantInternal PlantInternal { get; protected set; }
        public StandaloneIconPlantBox SIBox { get; set; }

        public InitPlantSIArgs([NotNull] IPlantInternal plantInternal, [NotNull] string luggageName, EventHandler closeComponentClick, EventHandler exitGardenClick)
        {
            if (plantInternal == null) throw new ArgumentNullException("plantInternal");
            if (luggageName == null) throw new ArgumentNullException("luggageName");
            PlantInternal = plantInternal;
            LuggageName = luggageName;
            CloseComponentClick = closeComponentClick;
            ExitGardenClick = exitGardenClick;
        }
    }
}
