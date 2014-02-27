#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering
{
  public interface IResultProvider
  {
    #region Public Events

    event EventHandler<ResultObtainedEventArgs> ResultObtained;

    #endregion

    #region Public Properties

    NotificationResult Result { get; }

    #endregion
  }
}