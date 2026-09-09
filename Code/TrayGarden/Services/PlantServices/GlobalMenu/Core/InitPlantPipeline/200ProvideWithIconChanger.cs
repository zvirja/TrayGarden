using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception.Services;
using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;

[UsedImplicitly]
public class ProvideWithIconChanger : IPipelineProcessor<InitPlantGMArgs>
{
  [UsedImplicitly]
  public virtual void Process(InitPlantGMArgs args)
  {
    var asExpected = args.PlantEx.GetFirstWorkhorseOfType<IChangesGlobalIcon>();
    if (asExpected == null)
    {
      return;
    }
    INotifyIconChangerMaster globalNotifyIconChanger = args.GlobalNotifyIconChanger;
    asExpected.StoreGlobalIconChangingAssignee(globalNotifyIconChanger);
    args.GMBox.GlobalNotifyIconChanger = globalNotifyIconChanger;
    args.IsNotifyIconChangerInUse = true;
  }
}
