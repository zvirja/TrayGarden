#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using TrayGarden.Configuration.ModernFactoryStuff.Parcers;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
  public interface IContentAssigner
  {
    #region Public Methods and Operators

    void AssignContent(XmlNode contentNode, object instance, Type instanceType, Func<Type, IParcer> valueParcerResolver);

    #endregion
  }
}