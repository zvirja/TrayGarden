using System;
using System.Reflection;
using System.Xml;
using TrayGarden.Configuration.ModernFactoryStuff.Parcers;

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
    public class MethodAssigner : IContentAssigner
    {
        public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType,
                                          Func<Type, IParcer> valueParcerResolver)
        {
            XmlNode nodeValue = contentNode.FirstChild;
            if (nodeValue == null)
                return;
            string nodeName = contentNode.Name;
            MethodInfo methodInfo = instanceType.GetMethod(nodeName);
            if (methodInfo == null)
                return;
            ParameterInfo[] methodParameters = methodInfo.GetParameters();
            if (methodParameters.Length != 1)
                return;
            Type firstParameterType = methodParameters[0].ParameterType;
            IParcer valueParcer = valueParcerResolver(firstParameterType);
            var contentValue = valueParcer.ParceNodeValue(nodeValue);
            if (!firstParameterType.IsInstanceOfType(contentValue))
                return;
            methodInfo.Invoke(instance, new[] {contentValue});
        }

        public static IContentAssigner Instance { get; protected set; }

        static MethodAssigner()
        {
            Instance = new MethodAssigner();
        }
    }
}