#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Reception.Services;
using TrayGarden.Services.FleaMarket.IconChanger;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class ProvideWithIconChanger
  {
    #region Public Methods and Operators

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

    #endregion
  }
}