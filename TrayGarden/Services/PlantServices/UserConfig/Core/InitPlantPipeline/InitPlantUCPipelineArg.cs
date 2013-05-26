using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Smorgasbord;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline
{
    public class InitPlantUCPipelineArg:PipelineArgs
    {
        public InitPlantUCPipelineArg(string luggageName, IPlantEx relatedPlant)
        {
            LuggageName = luggageName;
            RelatedPlant = relatedPlant;
        }

        public string LuggageName { get; set; }
        public IPlantEx RelatedPlant { get; set; }
        public ISettingsBox SettingBox { get; set; }
        public IUserSettingsBridgeMaster Bridge { get; set; }
        public List<IUserSettingMetadataMaster> SettingsMetadata { get; set; }
        public List<IUserSettingMaster> Settings { get; set; } 
        public IUserConfiguration Workhorse { get; set; }

        public UserConfigServicePlantBox PlantBox { get; set; }
    }
}
