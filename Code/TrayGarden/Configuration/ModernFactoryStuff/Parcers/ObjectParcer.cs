#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
  public class ObjectParcer : IParcer
  {
    #region Constructors and Destructors

    public ObjectParcer(ModernFactory modernFactoryInstance)
    {
      this.ModernFactoryInstance = modernFactoryInstance;
    }

    #endregion

    #region Properties

    protected ModernFactory ModernFactoryInstance { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual object ParceNodeValue(XmlNode nodeValue)
    {
      var instance = this.ModernFactoryInstance.GetObject(nodeValue);
      return instance;
    }

    #endregion
  }
}