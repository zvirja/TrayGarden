using System;

using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

[UsedImplicitly]
public class AssignIconModifier(Func<INotifyIconChangerMaster> iconChangerFactory) : IPipelineProcessor<InitPlantSIArgs>
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
    INotifyIconChangerMaster iconChanger = iconChangerFactory();
    iconChanger.Initialize(args.SIBox.NotifyIcon);
    iconRequirer.StoreIconChangingAssignee(iconChanger);
  }
}
