using System.Windows.Input;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces
{
  public interface IInformNotification:IResultProvider
  {
    string TextToDisplay { get; set; }
    TextDisplayOptions TextDisplayFont { get; set; }
  }
}