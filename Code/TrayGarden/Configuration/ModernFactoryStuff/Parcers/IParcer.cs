using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers;

public interface IParcer
{
  object ParceNodeValue(XmlNode nodeValue);
}