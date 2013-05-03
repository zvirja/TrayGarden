using System;
using System.Collections;
using System.Xml;
using TrayGarden.Configuration.ModernFactoryStuff.Parcers;

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
    public class ListAssigner : IContentAssigner
    {
        public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType,
                                          Func<Type, IParcer> valueParcerResolver)
        {
            XmlNodeList listContentNodes = GetListContentNodes(contentNode);
            if (listContentNodes.Count == 0)
                return;
            IList list = GetListObject(contentNode, instance, instanceType);
            if (list == null)
                return;
            AssignContentToList(list, listContentNodes,valueParcerResolver);
        }

        protected virtual void AssignContentToList(IList list, XmlNodeList contentNodes, Func<Type, IParcer> valueParcerResolver)
        {
            if (list == null)
                return;
            var listGenericArgument = GetListGenericArgumentType(list.GetType());
            if (listGenericArgument == null)
                return;
            IParcer parcer = valueParcerResolver(listGenericArgument);
            if (parcer == null)
                return;
            foreach (XmlNode contentNode in contentNodes)
            {
                object contentValue = parcer.ParceNodeValue(contentNode.FirstChild);
                if (contentValue == null)
                    contentValue = parcer.ParceNodeValue(contentNode);
                if (contentValue == null)
                    continue;
                if (!listGenericArgument.IsInstanceOfType(contentValue))
                    continue;
                list.Add(contentValue);
            }
        }

        protected virtual XmlNodeList GetListContentNodes(XmlNode contentNode)
        {
            return contentNode.ChildNodes;
        }

        protected virtual Type GetListGenericArgumentType(Type listType)
        {
            var genericArguments = listType.GetGenericArguments();
            if (genericArguments.Length != 1)
                return null;
            return genericArguments[0];
        }

        protected virtual IList GetListObject(XmlNode contentNode, object instance,Type instanceType)
        {
            var nodeName = contentNode.Name;
            var property = instanceType.GetProperty(nodeName);
            if (property == null)
                return null;
            if (!property.CanRead)
                return null;
            var listObj = property.GetValue(instance, null) as IList;
            return listObj;
        }


    }
}