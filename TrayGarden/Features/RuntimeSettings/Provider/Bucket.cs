﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TrayGarden.Features.RuntimeSettings.Provider
{
    [Serializable]
    [XmlRoot(ElementName = "bucket")]
    public class Bucket
    {
        [XmlArray(ElementName = "settings")]
        [XmlArrayItem(ElementName = "setting")]
        public List<StringStringPair> Settings { get; set; }


        [XmlArray(ElementName = "innerBuckets")]
        [XmlArrayItem(ElementName = "bucket", Type = typeof (Bucket))]
        public List<Bucket> InnerBuckets { get; set; }


        [XmlAttribute(AttributeName = "bucketName")]
        public string Name { get; set; }


        public Bucket()
        {
            Settings = new List<StringStringPair>();
            InnerBuckets = new List<Bucket>();
        }
    }
}