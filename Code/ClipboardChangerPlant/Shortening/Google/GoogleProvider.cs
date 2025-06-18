using System;
using System.IO;
using System.Net;

namespace ClipboardChangerPlant.Shortening.Google;

public class GoogleProvider : ShortenerProvider
{
  public string RequestUrl
  {
    get
    {
      return ConfigurationHelper.GetStringValue("RequestUrl", "https://www.googleapis.com/urlshortener/v1/url");
    }
  }

  public override bool TryShortUrl(string originalUrl, out string shortedUrl)
  {
    shortedUrl = ShortUrl(originalUrl);
    return !string.IsNullOrEmpty(shortedUrl);
  }

  protected string ShortUrl(string longUrl)
  {
    var requestObj = new RequestObject(longUrl);
    var postContent = SerializationHelper.SerializeToString(requestObj);
    var webRequest = PrepareRequest(postContent);
    try
    {
      var response = webRequest.GetResponse();
      ResponseObject responseObj = SerializationHelper.DeserializeObject<ResponseObject>(response.GetResponseStream());
      return responseObj.ID;
    }
    catch
    {
      return null;
    }
  }

  private string MakeUrl()
  {
    var uri = new UriBuilder(new Uri(RequestUrl));
    uri.Query = "key=" + ApiKey;
    return uri.ToString();
  }

  private WebRequest PrepareRequest(string postContent)
  {
    var wr = WebRequest.Create(MakeUrl());
    wr.ContentType = "application/json";
    wr.Method = "POST";
    using (var sw = new StreamWriter(wr.GetRequestStream()))
    {
      sw.Write(postContent);
    }
    return wr;
  }
}