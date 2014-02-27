#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

#endregion

namespace ClipboardChangerPlant.Shortening.Google
{
  public class GoogleProvider : ShortenerProvider
  {
    #region Public Properties

    public string RequestUrl
    {
      get
      {
        return this.ConfigurationHelper.GetStringValue("RequestUrl", "https://www.googleapis.com/urlshortener/v1/url");
      }
    }

    #endregion

    #region Public Methods and Operators

    public override bool TryShortUrl(string originalUrl, out string shortedUrl)
    {
      shortedUrl = this.ShortUrl(originalUrl);
      return !string.IsNullOrEmpty(shortedUrl);
    }

    #endregion

    #region Methods

    protected string ShortUrl(string longUrl)
    {
      var requestObj = new RequestObject(longUrl);
      var postContent = SerializationHelper.SerializeToString(requestObj);
      var webRequest = this.PrepareRequest(postContent);
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
      var uri = new UriBuilder(new Uri(this.RequestUrl));
      uri.Query = "key=" + this.ApiKey;
      return uri.ToString();
    }

    private WebRequest PrepareRequest(string postContent)
    {
      var wr = WebRequest.Create(this.MakeUrl());
      wr.ContentType = "application/json";
      wr.Method = "POST";
      using (var sw = new StreamWriter(wr.GetRequestStream()))
      {
        sw.Write(postContent);
      }
      return wr;
    }

    #endregion
  }
}