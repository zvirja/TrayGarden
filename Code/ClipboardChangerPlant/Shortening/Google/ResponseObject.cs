using System.Runtime.Serialization;

namespace ClipboardChangerPlant.Shortening.Google;

[DataContract]
internal class ResponseObject
{
  [DataMember(Name = "id")]
  public string ID { get; set; }

  [DataMember(Name = "kind")]
  public string Kind { get; set; }

  [DataMember(Name = "longUrl")]
  public string LongUrl { get; set; }
}