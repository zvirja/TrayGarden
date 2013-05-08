using System;
using System.Xml.Serialization;

namespace TrayGarden.RuntimeSettings.Provider
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