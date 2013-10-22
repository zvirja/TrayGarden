using System;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

namespace ClipboardChangerPlant.RequestHandling.Specialized
{
  public class JingRequestHandler : Clip2NetWithRequestHandler
  {
    public JingRequestHandler()
    {
      this.revertDialogHeader = "Jing value was transformed";
    }

    protected override string ExcractOriginalUrl(string pageBody)
    {
      int index = pageBody.IndexOf("<img class=\"embeddedObject\" src=\"", StringComparison.Ordinal);
      if (index < 0)
      {
        return null;
      }
      string str = pageBody.Substring(index + "<img class=\"embeddedObject\" src=\"".Length);
      int length = str.IndexOf("\"", StringComparison.Ordinal);
      return str.Substring(0, length);
    }

    protected override IResultProvider GetConfirmationDialog()
    {
      return new YesNoNotificationVM("Extract picture from Jing URL?");
    }

    protected override string GetEnabledSettingName()
    {
      return "Enable Jing monitor";
    }

    protected override string GetRevertConfirmatorSettingName()
    {
      return "Jing ask for revert";
    }
  }
}