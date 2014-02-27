#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces
{
  public interface IYesNoNotification : IResultProvider
  {
    #region Public Properties

    ImageDisplayOptions ButtonImagesDisplayOptions { get; set; }

    TextDisplayOptions ButtonTextsOptions { get; set; }

    ImageTextOrder ButtonsLayoutKind { get; set; }

    string HeaderText { get; set; }

    TextDisplayOptions HeaderTextOptions { get; set; }

    ICommand NoAction { get; set; }

    ImageSource NoButtonImage { get; set; }

    string NoButtonText { get; set; }

    ImageSource YesButtonImage { get; set; }

    string YesButtonText { get; set; }

    #endregion
  }
}