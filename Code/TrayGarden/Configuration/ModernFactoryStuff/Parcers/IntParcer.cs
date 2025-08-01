﻿using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers;

public class IntParcer : IParcer
{
  public virtual object ParceNodeValue(XmlNode nodeValue)
  {
    string value = nodeValue.InnerText;
    int intValue;
    if (int.TryParse(value, out intValue))
    {
      return intValue;
    }
    return null;
  }
}