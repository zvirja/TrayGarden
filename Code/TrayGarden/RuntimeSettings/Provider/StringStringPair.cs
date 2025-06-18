using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TrayGarden.RuntimeSettings.Provider
{
  [Serializable]
  public struct StringStringPair
  {
    public StringStringPair(string key, string value)
      : this()
    {
      this.Key = key;
      this.Value = value;
    }

    [XmlAttribute(AttributeName = "key")]
    public string Key { get; set; }

    [XmlAttribute(AttributeName = "value")]
    public string Value { get; set; }
  }
}