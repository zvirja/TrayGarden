using System.Windows.Media;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;

public interface IActionNotification : IResultProvider
{
  ImageSource ButtonImage { get; set; }

  ImageDisplayOptions ButtonImageDisplayOptions { get; set; }

  string ButtonText { get; set; }

  TextDisplayOptions ButtonTextDisplayStyle { get; set; }

  string HeaderText { get; set; }

  TextDisplayOptions HeaderTextDisplayStyle { get; set; }

  ImageTextOrder LayoutType { get; set; }
}