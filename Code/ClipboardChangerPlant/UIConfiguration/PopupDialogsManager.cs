using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Configuration.ApplicationConfiguration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

namespace ClipboardChangerPlant.UIConfiguration
{
  public class PopupDialogsManager : TrayGarden.Reception.Services.IGetPowerOfUserNotifications
  {
    #region static part

    public static PopupDialogsManager ActualManager { get; set; }

    static PopupDialogsManager()
    {
      ActualManager = new PopupDialogsManager();
    }

    #endregion


    public ILordOfNotifications LordOfNotifications { get; set; }

    public void StoreLordOfNotifications(ILordOfNotifications lordOfNotifications)
    {
      LordOfNotifications = lordOfNotifications;
    }
  }
}
