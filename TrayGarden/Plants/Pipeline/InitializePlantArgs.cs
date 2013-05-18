using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants.Pipeline
{
    public class InitializePlantArgs:PipelineArgs
    {
        public object PlantWorkhorse { get; protected set; }
        public IPlantEx ResolvedPlantEx { get; set; }
        public ISettingsBox RootSettingsBox { get; protected set; }

        public InitializePlantArgs(object plantWorkhorse, ISettingsBox rootSettingsBox)
        {
            PlantWorkhorse = plantWorkhorse;
            RootSettingsBox = rootSettingsBox;
        }
    }
}
