using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TrayGarden.Features.RuntimeSettings.Provider
{
    [Serializable]
    public struct StringStringPair
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        public StringStringPair(string key, string value)
            : this()
        {
            Key = key;
            Value = value;
        }
    }
}