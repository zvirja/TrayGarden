﻿using JetBrains.Annotations;

using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

[UsedImplicitly]
public class AssignIconModifier
{
  [UsedImplicitly]
  public virtual void Process(InitPlantSIArgs args)
  {
    var asIconModifyRequirer = args.PlantEx.GetFirstWorkhorseOfType<INeedToModifyIcon>();
    if (asIconModifyRequirer == null)
    {
      return;
    }
    AssignIconModifierToRequirer(args, asIconModifyRequirer);
  }

  protected virtual void AssignIconModifierToRequirer(InitPlantSIArgs args, INeedToModifyIcon iconRequirer)
  {
    INotifyIconChangerMaster iconChanger = HatcherGuide<INotifyIconChangerMaster>.CreateNewInstance();
    iconChanger.Initialize(args.SIBox.NotifyIcon);
    iconRequirer.StoreIconChangingAssignee(iconChanger);
  }
}