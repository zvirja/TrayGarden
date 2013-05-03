using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using TrayGarden.Configuration.ModernFactoryStuff.Parcers;
using TrayGarden.Helpers;

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

        #region List functionality

        protected virtual void AssignContentToList(IList list, XmlNodeList contentNodes,
                                                   Func<Type, IParcer> valueParcerResolver)
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

        protected virtual Type GetListGenericArgumentType(Type listType)
        {
            var genericArguments = listType.GetGenericArguments();
            if (genericArguments.Length != 1)
                return null;
            return genericArguments[0];
        }

        protected virtual IList GetListObject(XmlNode contentNode, object instance, Type instanceType)
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

        #endregion


        #region Templates functionality

        protected virtual XmlNodeList GetListContentNodes(XmlNode contentNode)
        {
            if(!SupportTemplating(contentNode))
                return contentNode.ChildNodes;
            return GetTemplateBasedContentNodes(contentNode);
        }

        protected virtual bool SupportTemplating(XmlNode contentNode)
        {
            XmlNode itemsNode = contentNode.SelectSingleNode("items");
            XmlNode templateNode = contentNode.SelectSingleNode("templates");
            return itemsNode != null && templateNode != null;
        }

        protected virtual XmlNodeList GetTemplateBasedContentNodes(XmlNode contentNode)
        {
            XmlNode itemsResolved = contentNode.SelectSingleNode("itemsresolved");
            if (itemsResolved != null)
                return itemsResolved.ChildNodes;
            var newResolvedItems = ResolveContentNodesFromTemplate(contentNode);
            itemsResolved = contentNode.OwnerDocument.CreateElement("itemsresolved");
            contentNode.AppendChild(itemsResolved);
            foreach (XmlNode newResolvedItem in newResolvedItems)
            {
                contentNode.AppendChild(newResolvedItem);
            }
            return contentNode.ChildNodes;
        }

        protected virtual List<XmlNode> ResolveContentNodesFromTemplate(XmlNode contentNode)
        {
            var result = new List<XmlNode>();
            XmlNode itemsNode = contentNode.SelectSingleNode("items");
            XmlNode templatesParentNode = contentNode.SelectSingleNode("templates");
            Debug.Assert(itemsNode != null && templatesParentNode != null);
            if (itemsNode.ChildNodes.Count == 0 || templatesParentNode.ChildNodes.Count == 0)
                return result;
            string defaultTemplate = XmlHelper.GetAttributeValue(templatesParentNode, "templateXPath");
            foreach (XmlNode itemNode in itemsNode.ChildNodes)
            {
                string itemTemplate = XmlHelper.GetAttributeValue(itemNode, "templateXPath");
                if (itemTemplate.IsNullOrEmpty())
                    itemTemplate = defaultTemplate;
                if (itemTemplate.IsNullOrEmpty())
                {
                    result.Add(itemNode);
                    continue;
                }
                XmlNode currentTemplateNode = GetTemplateByXPath(templatesParentNode, itemTemplate);
                var keyValuePairs = ExtractAllKeyValues(itemNode);
                var resultItemNode = GetItemNodeFromTemplate(currentTemplateNode, keyValuePairs);
                if(resultItemNode != null)
                    result.Add(resultItemNode);
            }
            return result;
        }

        protected virtual XmlNode GetTemplateByXPath(XmlNode templatesBaseNode, string xpath)
        {
            return templatesBaseNode.SelectSingleNode(xpath);
        }

        protected virtual Dictionary<string, string> ExtractAllKeyValues(XmlNode itemNode)
        {
            var result = new Dictionary<string, string>();
            if (itemNode.Attributes != null)
            {
                foreach (XmlAttribute attribute in itemNode.Attributes)
                {
                    Debug.Assert(!result.ContainsKey(attribute.Name));
                    result[attribute.Name] = attribute.Value;
                }
            }
            foreach (XmlNode innerNode in itemNode.ChildNodes)
            {
                Debug.Assert(!result.ContainsKey(innerNode.Name));
                result[innerNode.Name] = innerNode.InnerText;
            }
            return result;
        }

        protected virtual XmlNode GetItemNodeFromTemplate(XmlNode templateNode, Dictionary<string, string> variableValues)
        {
            //work with template node clone
            var templateClone = templateNode.CloneNode(true);
            var itemNode = templateClone.FirstChild;
            Debug.Assert(itemNode != null);
            SubstituteVariablesInItemNode(itemNode,variableValues);
            return itemNode;
        }

        protected virtual void SubstituteVariablesInItemNode(XmlNode itemNode, Dictionary<string, string> variableValues)
        {
            var varName = ExtractVariableNameFromNodeValue(itemNode);
            if (varName != null)
                itemNode.Value = variableValues.ContainsKey(varName) ? variableValues[varName] : string.Empty;
            if (itemNode.Attributes != null)
                foreach (XmlAttribute xmlAttribute in itemNode.Attributes)
                {
                    var variableName = ExtractVariableNameFromNodeValue(xmlAttribute);
                    if (variableName.IsNullOrEmpty())
                        continue;
                    xmlAttribute.Value = variableValues.ContainsKey(variableName) ? variableValues[variableName] : string.Empty;
                }
            foreach (XmlNode childNode in itemNode.ChildNodes)
                SubstituteVariablesInItemNode(childNode, variableValues);
        }

        protected virtual string ExtractVariableNameFromNodeValue(XmlNode node)
        {
            if (node == null)
                return null;
            var nodeValue = node.Value;
            if (nodeValue.IsNullOrEmpty())
                return null;
            if (!nodeValue.StartsWith("{") || !nodeValue.EndsWith("}"))
                return null;
            if (nodeValue.Length < 3)
                return null;
            return nodeValue.Substring(1, nodeValue.Length - 2);
        }

        #endregion

    }
}