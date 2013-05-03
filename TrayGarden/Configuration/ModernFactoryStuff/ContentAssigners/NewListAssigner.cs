using System;
using System.Collections;
using System.Collections.Generic;

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
    public class NewListAssigner : ListAssigner
    {
        protected override System.Collections.IList GetListObject(System.Xml.XmlNode contentNode, object instance, System.Type instanceType)
        {
            var nodeName = contentNode.Name;
            var property = instanceType.GetProperty(nodeName);
            if (property == null)
                return null;
            if (!property.CanWrite)
                return null;
            var propertyType = property.PropertyType;
            if (!typeof(IList).IsAssignableFrom(propertyType))
                return null;
            var listArgumentType = base.GetListGenericArgumentType(property.PropertyType);
            var listType = typeof (List<>).MakeGenericType(new[] {listArgumentType});
            var listObj = Activator.CreateInstance(listType) as IList;
            property.SetValue(instance, listObj, null);
            return listObj;
        }
    }
}