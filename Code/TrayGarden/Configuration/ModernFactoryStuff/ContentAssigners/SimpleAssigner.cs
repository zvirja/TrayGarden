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
  public class SimpleAssigner : IContentAssigner
  {
    #region Public Methods and Operators

    public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType, Func<Type, IParcer> valueParcerResolver)
    {
      var nodeName = contentNode.Name;
      XmlNode nodeValue = contentNode.FirstChild;
      if (nodeValue == null)
      {
        return;
      }
      var property = instanceType.GetProperty(nodeName);
      if (property == null)
      {
        return;
      }
      if (!property.CanWrite)
      {
        return;
      }
      var propertyType = property.PropertyType;
      IParcer parcer = valueParcerResolver(propertyType);
      if (parcer == null)
      {
        return;
      }
      object contentValue = parcer.ParceNodeValue(nodeValue);
      if (contentValue == null)
      {
        return;
      }
      if (!propertyType.IsInstanceOfType(contentValue))
      {
        return;
      }
      property.SetValue(instance, contentValue, null);
    }

    #endregion
  }
}