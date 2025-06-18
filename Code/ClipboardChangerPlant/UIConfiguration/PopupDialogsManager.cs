using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

namespace ClipboardChangerPlant.UIConfiguration;

public class PopupDialogsManager : TrayGarden.Reception.Services.IGetPowerOfUserNotifications
{
  static PopupDialogsManager()
  {
    ActualManager = new PopupDialogsManager();
  }

  public static PopupDialogsManager ActualManager { get; set; }

  public ILordOfNotifications LordOfNotifications { get; set; }

  public void StoreLordOfNotifications(ILordOfNotifications lordOfNotifications)
  {
    this.LordOfNotifications = lordOfNotifications;
  }
}