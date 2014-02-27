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
  internal class ResponseObject
  {
    #region Public Properties

    [DataMember(Name = "id")]
    public string ID { get; set; }

    [DataMember(Name = "kind")]
    public string Kind { get; set; }

    [DataMember(Name = "longUrl")]
    public string LongUrl { get; set; }

    #endregion
  }
}