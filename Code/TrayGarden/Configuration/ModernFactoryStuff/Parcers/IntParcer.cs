#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
  public class IntParcer : IParcer
  {
    #region Public Methods and Operators

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

    #endregion
  }
}