using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrayGarden.RuntimeSettings.Provider;

[Serializable]
[XmlRoot(ElementName = "bucket")]
public class Bucket
{
  public Bucket()
  {
    Settings = new List<StringStringPair>();
    InnerBuckets = new List<Bucket>();
  }

  [XmlArray(ElementName = "innerBuckets")]
  [XmlArrayItem(ElementName = "bucket", Type = typeof(Bucket))]
  public List<Bucket> InnerBuckets { get; set; }

  [XmlAttribute(AttributeName = "bucketName")]
  public string Name { get; set; }

  [XmlArray(ElementName = "settings")]
  [XmlArrayItem(ElementName = "setting")]
  public List<StringStringPair> Settings { get; set; }
}