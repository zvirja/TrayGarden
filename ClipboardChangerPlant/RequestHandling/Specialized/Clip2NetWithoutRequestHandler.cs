using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
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
  public class Clip2NetWithoutRequestHandler : RequestHandler
  {
    protected UIDialogConfirmator RevertConfirmator { get; set; }
    protected UIDialogConfirmator ExecuteChecker { get; set; }

    public Clip2NetWithoutRequestHandler()
    {
      RevertConfirmator = RegisterUIDialogConfirmator("Ask Clip2Net revert (non req)", GetRevertDialog);
      ExecuteChecker = RegisterUIDialogConfirmator("Enable Clip2Net monitor (non req)", () => null);
    }

    public override bool PreExecute(string operableUrl, bool isClipboardRequest)
    {
      if (!ExecuteChecker.ConfirmationSetting.BoolValue)
        return false;
      return base.PreExecute(operableUrl, isClipboardRequest);
    }

    public override bool TryProcess(string inputValue, out string result)
    {
      result = null;
      try
      {
        result = ResolveAsXml(inputValue);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public override bool PostmortemRevertValue(string currentUrl, string originalUrl, bool isClipboardRequest)
    {
      if (RevertConfirmator.ConfirmThroughUI() == true)
        return true;
      return base.PostmortemRevertValue(currentUrl, originalUrl, isClipboardRequest);
    }

    protected virtual string ResolveAsXml(string inputValue)
    {
      var root = XElement.Parse(inputValue);
      var imgNode = root.Element("img");
      return imgNode.Attribute("src").Value;
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
