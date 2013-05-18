using System;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.GlobalMenu.Smorgasbord;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class ProvideWithIconChanger
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantGMArgs args)
        {
            var asExpected = args.PlantEx.Workhorse as IChangesGlobalIcon;
            if (asExpected == null)
            {
                return;
            }
            asExpected.SetGlobalIconChangingAssignee(args.GlobalNotifyIconChanger);
        }
    }
}
