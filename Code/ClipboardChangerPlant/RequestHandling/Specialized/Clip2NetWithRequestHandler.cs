using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using JetBrains.Annotations;

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
        var pageBody = this.GetBody(inputValue);
        var originalUrl = this.ExcractOriginalUrl(pageBody);
        if (string.IsNullOrEmpty(originalUrl))
        {
          return false;
        }
        result = originalUrl;
        return true;
      }
      catch (Exception)
      {
        result = inputValue;
        return false;
      }
    }

    protected virtual string ExcractOriginalUrl(string pageBody)
    {
      var searchString = "<a class=\"image-down-file\" href=\"http://clip2net.com/clip/";
      var payloadPart = "http://clip2net.com/clip/";
      var indexOf = pageBody.IndexOf(searchString);
      if (indexOf < 0)
      {
        return null;
      }
      var contentStart = pageBody.Substring(indexOf + searchString.Length - payloadPart.Length);
      var endIndex = contentStart.IndexOf("\"");
      string validContent = contentStart.Substring(0, endIndex);

      //remove &fd=1 param which causes download
      const string downloadSuffix = "&fd=1";
      if (validContent.EndsWith(downloadSuffix))
      {
        validContent = validContent.Substring(0, validContent.Length - downloadSuffix.Length);
      }

      return validContent;
    }

    protected virtual string GetBody(string url)
    {
      var request = WebRequest.Create(url);
      var response = request.GetResponse();
      var result = new StreamReader(response.GetResponseStream()).ReadToEnd();
      return result;
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