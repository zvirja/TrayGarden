using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class CreateSettingsBox
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantGMArgs args)
        {
            var settingsBox = args.PlantInternal.MySettingsBox.GetSubBox("GlobalMenuService");
            args.GMBox.SettingsBox = settingsBox;
        }
    }
}
