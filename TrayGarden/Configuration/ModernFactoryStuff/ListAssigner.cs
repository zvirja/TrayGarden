using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TrayGarden.Helpers;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public class ListAssigner : IContentAssigner
    {
        public static IContentAssigner Instance { get; protected set; }

        static ListAssigner()
        {
            Instance = new ListAssigner();
        }

        public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType,
                                          Func<Type, IParcer> valueParcerResolver)
        {
            var nodeName = contentNode.Name;
            XmlNodeList innerNodes = contentNode.ChildNodes;
            if (innerNodes.Count == 0)
                return;
            var property = instanceType.GetProperty(nodeName);
            if (property == null)
                return;
            if (!property.CanRead)
                return;
            var listObj = property.GetValue(instance, null) as IList;
            if (listObj == null)
                return;
            var listGenericArgument = listObj.GetType().GetGenericArguments()[0];
            IParcer parcer = valueParcerResolver(listGenericArgument);
            if (parcer == null)
                return;
            foreach (XmlNode innerNode in innerNodes)
            {
                object contentValue = parcer.ParceNodeValue(innerNode.FirstChild);
                if (contentValue == null)
                    contentValue = parcer.ParceNodeValue(innerNode);
                if (contentValue == null)
                    continue;
                if (!listGenericArgument.IsInstanceOfType(contentValue))
                    continue;
                listObj.Add(contentValue);
            }
        }
    }
}