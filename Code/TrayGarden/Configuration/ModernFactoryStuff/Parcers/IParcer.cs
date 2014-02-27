#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
  public interface IParcer
  {
    #region Public Methods and Operators

    object ParceNodeValue(XmlNode nodeValue);

    #endregion
  }
}