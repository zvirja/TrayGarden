namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

public class NotificationResult
{
  public NotificationResult(ResultCode code)
    : this(code, null)
  {
  }

  public NotificationResult(ResultCode code, object customData)
  {
    Code = code;
    CustomData = customData;
  }

  public ResultCode Code { get; set; }

  public object CustomData { get; set; }
}