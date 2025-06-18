using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ClipboardChangerPlant.Shortening.Google;

[DataContract]
internal class RequestObject
{
  public RequestObject()
  {
  }

  public RequestObject(string longUrl)
  {
    this.LongUrl = longUrl;
  }

  [DataMember(Name = "longUrl")]
  public string LongUrl { get; set; }
}