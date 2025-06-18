using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
  public class StringParcer : IParcer
  {
    public virtual object ParceNodeValue(XmlNode nodeValue)
    {
      var value = nodeValue.InnerText;
      return value;
    }
  }
}