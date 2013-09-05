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
  public class Clip2NetWithRequestHandler : Clip2NetWithoutRequestHandler
  {
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

    protected override string GetEnabledSettingName()
    {
      return "Enable Clip2Net(req) monitor";
    }

    protected override string GetRevertConfirmatorSettingName()
    {
      return "Clip2Net(req) ask for revert";
    }

  }
}
