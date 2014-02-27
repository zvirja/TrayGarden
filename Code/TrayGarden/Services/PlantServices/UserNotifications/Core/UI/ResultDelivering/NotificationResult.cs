#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering
{
  public class NotificationResult
  {
    #region Constructors and Destructors

    public NotificationResult(ResultCode code)
      : this(code, null)
    {
    }

    public NotificationResult(ResultCode code, object customData)
    {
      this.Code = code;
      this.CustomData = customData;
    }

    #endregion

    #region Public Properties

    public ResultCode Code { get; set; }

    public object CustomData { get; set; }

    #endregion
  }
}