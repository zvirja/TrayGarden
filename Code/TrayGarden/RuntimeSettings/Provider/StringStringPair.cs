#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace TrayGarden.RuntimeSettings.Provider
{
  [Serializable]
  public struct StringStringPair
  {
    #region Constructors and Destructors

    public StringStringPair(string key, string value)
      : this()
    {
      this.Key = key;
      this.Value = value;
    }

    #endregion

    #region Public Properties

    [XmlAttribute(AttributeName = "key")]
    public string Key { get; set; }

    [XmlAttribute(AttributeName = "value")]
    public string Value { get; set; }

    #endregion
  }
}