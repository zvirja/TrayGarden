#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces
{
  public interface IInformNotification : IResultProvider
  {
    #region Public Properties

    TextDisplayOptions TextDisplayFont { get; set; }

    string TextToDisplay { get; set; }

    #endregion
  }
}