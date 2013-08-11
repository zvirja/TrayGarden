using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Plants.Intergration
{
    public class AutoLoadPropertyAwarePlayer:ConfigurationAwarePlayer
    {
        public override bool BoolValue
        {
            get
            {
                return HatcherGuide<IGardenbed>.Instance.AutoDetectPlants;
            }
            set
            {
                HatcherGuide<IGardenbed>.Instance.AutoDetectPlants = value;
            }
        }
        public AutoLoadPropertyAwarePlayer([NotNull] string settingName, string settingDescription) : base(settingName, false, false)
        {
            base.SettingDescription = settingDescription;
        }
    }
}
