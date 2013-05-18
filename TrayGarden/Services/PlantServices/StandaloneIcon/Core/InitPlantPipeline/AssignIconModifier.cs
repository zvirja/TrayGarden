using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.StandaloneIcon.Smorgasbord;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class AssignIconModifier
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantSIArgs args)
        {
            var asIconModifyRequirer = args.PlantEx.Workhorse as INeedToModifyIcon;
            if (asIconModifyRequirer == null)
                return;
            AssignIconModifierToRequirer(args, asIconModifyRequirer);
        }

        protected virtual void AssignIconModifierToRequirer(InitPlantSIArgs args, INeedToModifyIcon iconRequirer)
        {
            INotifyIconChangerMaster iconChanger = HatcherGuide<INotifyIconChangerMaster>.CreateNewInstance();
            iconChanger.Initialize(args.SIBox.NotifyIcon);
            iconRequirer.SetIconChangingAssignee(iconChanger);
        }
    }
}
