using System;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
    public class InitPlantGMArgs:PipelineArgs
    {
        public string LuggageName { get; set; }
        public IPlantInternal PlantInternal { get; protected set; }
        public INotifyIconChangerClient GlobalNotifyIconChanger { get; set; }

        public GlobalMenuPlantBox GMBox { get; set; }


        public InitPlantGMArgs([NotNull] IPlantInternal plantInternal, [NotNull] string luggageName,
                               [NotNull] INotifyIconChangerClient globalNotifyIconChanger)
        {
            if (plantInternal == null) throw new ArgumentNullException("plantInternal");
            if (luggageName == null) throw new ArgumentNullException("luggageName");
            if (globalNotifyIconChanger == null) throw new ArgumentNullException("globalNotifyIconChanger");
            PlantInternal = plantInternal;
            LuggageName = luggageName;
            GlobalNotifyIconChanger = globalNotifyIconChanger;
        }
    }
}
