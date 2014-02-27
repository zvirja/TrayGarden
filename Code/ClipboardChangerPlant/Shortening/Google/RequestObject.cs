#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

#endregion

namespace ClipboardChangerPlant.Shortening.Google
{
  [DataContract]
  internal class RequestObject
  {
    #region Constructors and Destructors

    public RequestObject()
    {
    }

    public RequestObject(string longUrl)
    {
      this.LongUrl = longUrl;
    }

    #endregion

    #region Public Properties

    [DataMember(Name = "longUrl")]
    public string LongUrl { get; set; }

    #endregion
  }
}