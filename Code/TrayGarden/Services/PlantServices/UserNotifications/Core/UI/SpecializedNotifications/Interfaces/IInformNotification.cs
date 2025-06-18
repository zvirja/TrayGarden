using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;

public interface IInformNotification : IResultProvider
{
  TextDisplayOptions TextDisplayFont { get; set; }

  string TextToDisplay { get; set; }
}