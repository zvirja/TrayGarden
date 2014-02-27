#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering
{
  public class ResultObtainedEventArgs : EventArgs
  {
    #region Constructors and Destructors

    public ResultObtainedEventArgs(NotificationResult result)
    {
      this.Result = result;
    }

    #endregion

    #region Public Properties

    public NotificationResult Result { get; set; }

    #endregion
  }
}