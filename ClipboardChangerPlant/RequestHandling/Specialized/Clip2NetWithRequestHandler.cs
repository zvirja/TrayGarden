using System;
using System.IO;
using System.Net;

namespace ClipboardChangerPlant.RequestHandling.Specialized
{
  public class Clip2NetWithRequestHandler : RequestHandler
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

    private string GetBody(string url)
    {
      var request = WebRequest.Create(url);
      var response = request.GetResponse();
      var result = new StreamReader(response.GetResponseStream()).ReadToEnd();
      return result;
    }

    private string ExcractOriginalUrl(string pageBody)
    {
      var searchString = "name=\"twitter:image\" content=\"";
      var indexOf = pageBody.IndexOf(searchString);
      if (indexOf < 0)
        return null;
      var contentStart = pageBody.Substring(indexOf + 30);
      var endIndex = contentStart.IndexOf("\">");
      var validContent = contentStart.Substring(0, endIndex);
      return validContent;
    }
  }
}
