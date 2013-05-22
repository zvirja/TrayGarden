using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Hallway;
using TrayGarden.Pipelines.Engine;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants.Pipeline
{
    public class InitializePlantArgs:PipelineArgs
    {
        public object PlantObject { get; protected set; }
        public List<object> Workhorses { get; set; }
        public string PlantID { get; set; }
        public IPlant IPlantObject { get; set; }
        public IPlantEx ResolvedPlantEx { get; set; }
        public ISettingsBox PlantSettingsBox { get; set; }
        public ISettingsBox RootSettingsBox { get; protected set; }


        public InitializePlantArgs(object plant, ISettingsBox rootSettingsBox)
        {
            PlantObject = plant;
            RootSettingsBox = rootSettingsBox;
        }
    }
}
