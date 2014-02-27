#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces
{
  public interface IActionNotification : IResultProvider
  {
    #region Public Properties

    ImageSource ButtonImage { get; set; }

    ImageDisplayOptions ButtonImageDisplayOptions { get; set; }

    string ButtonText { get; set; }

    TextDisplayOptions ButtonTextDisplayStyle { get; set; }

    string HeaderText { get; set; }

    TextDisplayOptions HeaderTextDisplayStyle { get; set; }

    ImageTextOrder LayoutType { get; set; }

    #endregion
  }
}