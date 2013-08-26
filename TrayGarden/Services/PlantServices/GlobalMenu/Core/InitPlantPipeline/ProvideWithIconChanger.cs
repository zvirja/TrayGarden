using System;
using JetBrains.Annotations;
using TrayGarden.Reception.Services;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class ProvideWithIconChanger
  {
    [UsedImplicitly]
    public virtual void Process(InitPlantGMArgs args)
    {
      var asExpected = args.PlantEx.GetFirstWorkhorseOfType<IChangesGlobalIcon>();
      if (asExpected == null)
      {
        return;
      }
      asExpected.StoreGlobalIconChangingAssignee(args.GlobalNotifyIconChanger);
    }
  }
}
