using System.Windows.Input;
using System.Windows.Media;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces
{
  public interface IYesNoNotification:IResultProvider
  {
    string YesButtonText { get; set; }
    ImageSource YesButtonImage { get; set; }
    string NoButtonText { get; set; }
    ImageSource NoButtonImage { get; set; }
    ICommand NoAction { get; set; }
    string HeaderText { get; set; }
    TextDisplayOptions HeaderTextOptions { get; set; }
    ImageTextOrder ButtonsLayoutKind { get; set; }
    ImageDisplayOptions ButtonImagesDisplayOptions { get; set; }
    TextDisplayOptions ButtonTextsOptions { get; set; }
  }
}