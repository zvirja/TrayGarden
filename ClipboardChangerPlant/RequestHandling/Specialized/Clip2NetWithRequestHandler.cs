using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;
using ClipboardChangerPlant.Properties;
using ClipboardChangerPlant.UIConfiguration;
using JetBrains.Annotations;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;

namespace ClipboardChangerPlant.RequestHandling.Specialized
{
  [UsedImplicitly]
  public class Clip2NetWithRequestHandler : RequestHandler
  {
    protected UIDialogConfirmator ExecuteChecker { get; set; }
    protected UIDialogConfirmator RevertConfirmator { get; set; }


    public Clip2NetWithRequestHandler()
    {
      RevertConfirmator = RegisterUIDialogConfirmator("Ask Clip2Net revert (req)", GetRevertDialog);
      ExecuteChecker = RegisterUIDialogConfirmator("Enable Clip2Net monitor (req)", () => null);
    }

    public override bool TryProcess(string inputValue, out string result)
    {
      try
      {
        result = inputValue;
        var pageBody = GetBody(inputValue);
        var originalUrl = ExcractOriginalUrl(pageBody);
        if (string.IsNullOrEmpty(originalUrl))
          return false;
        result = originalUrl;
        return true;
      }
      catch (Exception)
      {
        result = inputValue;
        return false;
      }

    }

    protected virtual string GetBody(string url)
    {
      var request = WebRequest.Create(url);
      var response = request.GetResponse();
      var result = new StreamReader(response.GetResponseStream()).ReadToEnd();
      return result;
    }

    protected virtual string ExcractOriginalUrl(string pageBody)
    {
      var searchString = "<img src=\"http://clip2net.com/clip/";
      var indexOf = pageBody.IndexOf(searchString);
      if (indexOf < 0)
        return null;
      var contentStart = pageBody.Substring(indexOf + 10);
      var endIndex = contentStart.IndexOf("\"");
      var validContent = contentStart.Substring(0, endIndex);
      return validContent;
    }

    protected virtual IResultProvider GetRevertDialog()
    {
      IActionNotification revertDialog = RevertConfirmator.LordOfNotifications.CreateActionNotification(
        "Clip2Net value was transformed", "Revert value");
      revertDialog.LayoutType = ImageTextOrder.HorizontalTextImage;

      TextDisplayOptions headerTextDisplayStyle = revertDialog.HeaderTextDisplayStyle;
      headerTextDisplayStyle.Margins = new Thickness(5, 0, 0, 10);
      headerTextDisplayStyle.Size = 14;

      revertDialog.ButtonImage = GetUndoImage();
      ImageDisplayOptions imageDisplayOptions = revertDialog.ButtonImageDisplayOptions;
      imageDisplayOptions.Margins = new Thickness(20, 0, 0, 0);
      imageDisplayOptions.Height = imageDisplayOptions.Width = 48;

      revertDialog.ButtonTextDisplayStyle.Size = 18;
      revertDialog.ButtonTextDisplayStyle.Margins = new Thickness(15, 0, 0, 0);
      return revertDialog;
    }

    protected virtual ImageSource GetUndoImage()
    {
      var bitmap = Resources.undoImage48;
      return ImageHelper.GetBitmapImageFromBitmapThreadSafe(bitmap, ImageFormat.Png);
    }

  }
}
