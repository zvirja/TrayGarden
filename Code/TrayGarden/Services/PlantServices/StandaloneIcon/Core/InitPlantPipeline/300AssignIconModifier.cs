#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class AssignIconModifier
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantSIArgs args)
    {
      var asIconModifyRequirer = args.PlantEx.GetFirstWorkhorseOfType<INeedToModifyIcon>();
      if (asIconModifyRequirer == null)
      {
        return;
      }
      this.AssignIconModifierToRequirer(args, asIconModifyRequirer);
    }

    #endregion

    #region Methods

    protected virtual void AssignIconModifierToRequirer(InitPlantSIArgs args, INeedToModifyIcon iconRequirer)
    {
      INotifyIconChangerMaster iconChanger = HatcherGuide<INotifyIconChangerMaster>.CreateNewInstance();
      iconChanger.Initialize(args.SIBox.NotifyIcon);
      iconRequirer.StoreIconChangingAssignee(iconChanger);
    }

    #endregion
  }
}