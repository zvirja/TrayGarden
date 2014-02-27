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
  public class ObjectFactoryAssigner : IContentAssigner
  {
    #region Public Methods and Operators

    public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType, Func<Type, IParcer> valueParcerResolver)
    {
      var objectFactoryInstance = instance as ModernFactory.ObjectFactory;
      if (objectFactoryInstance == null)
      {
        throw new Exception("Unexpected value");
      }
      var contentNodeClone = contentNode.Clone();
      var typeAttribute = contentNodeClone.Attributes["type"];
      if (typeAttribute == null)
      {
        throw new Exception("Unexpected value");
      }
      var indexOfColon = typeAttribute.Value.IndexOf(":");
      if (indexOfColon == -1)
      {
        throw new Exception("Unexpected value");
      }
      var newTypeValue = typeAttribute.Value.Substring(indexOfColon + 1);
      typeAttribute.Value = newTypeValue;
      objectFactoryInstance.Initialize(contentNodeClone);
    }

    #endregion
  }
}