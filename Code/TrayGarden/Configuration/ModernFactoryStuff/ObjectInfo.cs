#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff
{
  public class ObjectInfo
  {
    #region Constructors and Destructors

    public ObjectInfo(object instance, XmlNode configurationNode, bool isSingleton, bool isPrototype)
    {
      this.Instance = instance;
      this.ConfigurationNode = configurationNode;
      this.IsPrototype = isPrototype;
      this.IsSingleton = isSingleton;
    }

    #endregion

    #region Public Properties

    public XmlNode ConfigurationNode { get; set; }

    public object Instance { get; set; }

    public bool IsPrototype { get; set; }

    public bool IsSingleton { get; set; }

    #endregion
  }
}