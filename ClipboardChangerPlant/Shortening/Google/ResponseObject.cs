using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ClipboardChangerPlant.Shortening.Google
{
  [DataContract]
  class ResponseObject
  {
    [DataMember(Name = "kind")]
    public string Kind { get; set; }

    [DataMember(Name = "id")]
    public string ID { get; set; }

    [DataMember(Name = "longUrl")]
    public string LongUrl { get; set; }

  }
}
