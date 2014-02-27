#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering
{
  public enum ResultCode
  {
    Unspecified,

    NoReaction,

    OK,

    Yes,

    No,

    Close,

    PermanentlyClose,

    Custom
  }
}