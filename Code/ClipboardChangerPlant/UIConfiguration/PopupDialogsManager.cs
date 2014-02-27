#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

#endregion

namespace ClipboardChangerPlant.UIConfiguration
{
  public class PopupDialogsManager : TrayGarden.Reception.Services.IGetPowerOfUserNotifications
  {
    #region Constructors and Destructors

    static PopupDialogsManager()
    {
      ActualManager = new PopupDialogsManager();
    }

    #endregion

    #region Public Properties

    public static PopupDialogsManager ActualManager { get; set; }

    public ILordOfNotifications LordOfNotifications { get; set; }

    #endregion

    #region Public Methods and Operators

    public void StoreLordOfNotifications(ILordOfNotifications lordOfNotifications)
    {
      this.LordOfNotifications = lordOfNotifications;
    }

    #endregion
  }
}