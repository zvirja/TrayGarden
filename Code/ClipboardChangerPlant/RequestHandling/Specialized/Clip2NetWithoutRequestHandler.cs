#region

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

using ClipboardChangerPlant.Properties;

using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;

#endregion

namespace ClipboardChangerPlant.RequestHandling.Specialized
{
  [UsedImplicitly]
  public class Clip2NetWithoutRequestHandler : RequestHandlerWithUIConfirmation
  {
    #region Fields

    protected string revertDialogHeader = "Clip2Net value was transformed";

    #endregion

    #region Public Methods and Operators

    public override bool TryProcess(string inputValue, out string result)
    {
      result = null;
      try
      {
        result = this.ResolveAsXml(inputValue);
        return true;
      }
      catch
      {
        return false;
      }
    }

    #endregion

    #region Methods

    protected override string GetEnabledSettingName()
    {
      return "Enable Clip2Net(non req) monitor";
    }

    protected override string GetRevertConfirmatorSettingName()
    {
      return "Clip2Net(non req) ask for revert";
    }

    protected override IResultProvider GetRevertDialog()
    {
      IActionNotification revertDialog =
        this.RevertConfirmator.LordOfNotifications.CreateActionNotification(this.revertDialogHeader, "Revert value");
      revertDialog.LayoutType = ImageTextOrder.HorizontalTextImage;

      TextDisplayOptions headerTextDisplayStyle = revertDialog.HeaderTextDisplayStyle;
      headerTextDisplayStyle.Margins = new Thickness(5, 0, 0, 10);
      headerTextDisplayStyle.Size = 14;

      revertDialog.ButtonImage = this.GetUndoImage();
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

    protected virtual string ResolveAsXml(string inputValue)
    {
      var root = XElement.Parse(inputValue);
      var imgNode = root.Element("img");
      return imgNode.Attribute("src").Value;
    }

    #endregion
  }
}