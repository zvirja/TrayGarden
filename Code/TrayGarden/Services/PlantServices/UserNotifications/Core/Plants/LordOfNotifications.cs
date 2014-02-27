#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Plants
{
  public class LordOfNotifications : ILordOfNotifications
  {
    #region Constructors and Destructors

    public LordOfNotifications(UserNotificationsServicePlantBox relatedPlantBox)
    {
      this.RelatedPlantBox = relatedPlantBox;
    }

    #endregion

    #region Properties

    protected UserNotificationsServicePlantBox RelatedPlantBox { get; set; }

    #endregion

    #region Public Methods and Operators

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
      if (!this.RelatedPlantBox.IsEnabled || !this.RelatedPlantBox.RelatedPlantEx.IsEnabled)
      {
        return new FakeNotificationResultCourier();
      }
      return HatcherGuide<IUserNotificationsGate>.Instance.EnqueueToShow(
        notificationBlank,
        this.RelatedPlantBox.RelatedPlantEx.Plant.HumanSupportingName);
    }

    #endregion
  }
}