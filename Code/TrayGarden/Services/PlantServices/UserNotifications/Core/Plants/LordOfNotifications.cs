using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;

public class LordOfNotifications : ILordOfNotifications
{
  public LordOfNotifications(UserNotificationsServicePlantBox relatedPlantBox)
  {
    RelatedPlantBox = relatedPlantBox;
  }

  protected UserNotificationsServicePlantBox RelatedPlantBox { get; set; }

  public virtual IActionNotification CreateActionNotification(string headerText, string buttonText)
  {
    return new ActionNotificationVM(headerText, buttonText);
  }

  public virtual IInformNotification CreateInformNotification(string textToDisplay)
  {
    return new InformNotificationVM(textToDisplay);
  }

  public virtual IYesNoNotification CreateYesNoNotification(string headerText)
  {
    return new YesNoNotificationVM(headerText);
  }

  public virtual INotificationResultCourier DisplayNotification(IResultProvider notificationBlank)
  {
    if (!RelatedPlantBox.IsEnabled || !RelatedPlantBox.RelatedPlantEx.IsEnabled)
    {
      return new FakeNotificationResultCourier();
    }
    return HatcherGuide<IUserNotificationsGate>.Instance.EnqueueToShow(
      notificationBlank,
      RelatedPlantBox.RelatedPlantEx.Plant.HumanSupportingName);
  }
}