#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
  public class StringParcer : IParcer
  {
    #region Public Methods and Operators

    public virtual object ParceNodeValue(XmlNode nodeValue)
    {
      var value = nodeValue.InnerText;
      return value;
    }

    #endregion
  }
}