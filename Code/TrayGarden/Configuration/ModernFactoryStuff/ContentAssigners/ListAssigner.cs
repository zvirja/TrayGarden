using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

using TrayGarden.Configuration.ModernFactoryStuff.Parcers;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners;

public class ListAssigner : IContentAssigner
{
  public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType, Func<Type, IParcer> valueParcerResolver)
  {
    XmlNodeList listContentNodes = this.GetListContentNodes(contentNode);
    if (listContentNodes.Count == 0)
    {
      return;
    }
    IList list = this.GetListObject(contentNode, instance, instanceType);
    if (list == null)
    {
      return;
    }
    this.AssignContentToList(list, listContentNodes, valueParcerResolver);
  }

  protected virtual void AssignContentToList(IList list, XmlNodeList contentNodes, Func<Type, IParcer> valueParcerResolver)
  {
    Assert.ArgumentNotNull(list, "list");
    var listGenericArgument = this.GetListGenericArgumentType(list.GetType());
    if (listGenericArgument == null)
    {
      throw new Exception("Unexpected value");
    }
    IParcer parcer = valueParcerResolver(listGenericArgument);
    if (parcer == null)
    {
      return;
    }
    foreach (XmlNode contentNode in contentNodes)
    {
      try
      {
        object contentValue = null;
        if (contentNode.FirstChild != null)
        {
          contentValue = parcer.ParceNodeValue(contentNode.FirstChild);
        }
        if (contentValue == null)
        {
          contentValue = parcer.ParceNodeValue(contentNode);
        }
        if (contentValue == null)
        {
          continue;
        }
        if (!listGenericArgument.IsInstanceOfType(contentValue))
        {
          continue;
        }
        list.Add(contentValue);
      }
      catch (Exception ex)
      {
        Log.Error("Can't parce node value", ex, this);
      }
    }
  }

  protected bool CheckAndInsertSubtree(XmlNode currentNode, XmlNode originalItemNode)
  {
    string xpathfromAttributeName = "xpathfrom";
    var xpathFrom = XmlHelper.GetAttributeValue(currentNode, xpathfromAttributeName);
    if (xpathFrom.IsNullOrEmpty())
    {
      return false;
    }
    currentNode.Attributes.Remove(currentNode.Attributes[xpathfromAttributeName]);
    var nodesFromXPath = originalItemNode.SelectNodes(xpathFrom);
    if (nodesFromXPath == null)
    {
      return true;
    }
    foreach (XmlNode node in nodesFromXPath)
    {
      currentNode.AppendChild(node.Clone());
    }
    return true;
  }

  protected virtual Dictionary<string, string> ExtractAllKeyValues(XmlNode itemNode)
  {
    var result = new Dictionary<string, string>();
    if (itemNode.Attributes != null)
    {
      foreach (XmlAttribute attribute in itemNode.Attributes)
      {
        /*Debug.Assert(!result.ContainsKey(attribute.Name));*/
        result[attribute.Name] = attribute.Value;
      }
    }
    foreach (XmlNode innerNode in itemNode.ChildNodes)
    {
      /*Debug.Assert(!result.ContainsKey(innerNode.Name));*/
      result[innerNode.Name] = innerNode.InnerText;
    }
    return result;
  }

  protected virtual string ExtractVariableNameFromNodeValueSoft(XmlNode node)
  {
    if (node == null)
    {
      return null;
    }
    var nodeValue = node.Value;
    if (nodeValue.IsNullOrEmpty())
    {
      return null;
    }
    var startIndex = nodeValue.IndexOf("{", System.StringComparison.Ordinal);
    var endIndex = nodeValue.LastIndexOf("}", System.StringComparison.Ordinal);

    if (startIndex < 0 || endIndex < 0 || endIndex < startIndex)
    {
      return null;
    }
    if (nodeValue.Length < 3)
    {
      return null;
    }
    var retValue = nodeValue.Substring(startIndex + 1, endIndex - startIndex - 1);
    return retValue;
  }

  protected virtual string ExtractVariableNameFromNodeValueStrong(XmlNode node)
  {
    if (node == null)
    {
      return null;
    }
    var nodeValue = node.Value;
    if (nodeValue.IsNullOrEmpty())
    {
      return null;
    }
    if (!nodeValue.StartsWith("{") || !nodeValue.EndsWith("}"))
    {
      return null;
    }
    if (nodeValue.Length < 3)
    {
      return null;
    }
    return nodeValue.Substring(1, nodeValue.Length - 2);
  }

  protected virtual XmlNode GetItemNodeFromTemplate(
    XmlNode templateNode,
    Dictionary<string, string> variableValues,
    XmlNode originalItemNode)
  {
    //work with template node clone
    var templateClone = templateNode.CloneNode(true);
    var itemNode = templateClone.FirstChild;
    Debug.Assert(itemNode != null);
    this.SubstituteVariablesInItemNode(itemNode, variableValues, originalItemNode);
    return itemNode;
  }

  protected virtual XmlNodeList GetListContentNodes(XmlNode contentNode)
  {
    if (!this.SupportTemplating(contentNode))
    {
      return contentNode.ChildNodes;
    }
    return this.GetTemplateBasedContentNodes(contentNode);
  }

  protected virtual Type GetListGenericArgumentType(Type listType)
  {
    var genericArguments = listType.GetGenericArguments();
    if (genericArguments.Length != 1)
    {
      throw new Exception("Unexpected value");
    }
    return genericArguments[0];
  }

  protected virtual IList GetListObject(XmlNode contentNode, object instance, Type instanceType)
  {
    var nodeName = contentNode.Name;
    var property = instanceType.GetProperty(nodeName);
    if (property == null)
    {
      return null;
    }
    if (!property.CanRead)
    {
      return null;
    }
    var listObj = property.GetValue(instance, null) as IList;
    return listObj;
  }

  protected virtual XmlNodeList GetTemplateBasedContentNodes(XmlNode contentNode)
  {
    XmlNode itemsResolved = contentNode.SelectSingleNode("itemsresolved");
    if (itemsResolved != null)
    {
      return itemsResolved.ChildNodes;
    }
    var newResolvedItems = this.ResolveContentNodesFromTemplate(contentNode);
    itemsResolved = contentNode.OwnerDocument.CreateElement("itemsresolved");
    contentNode.AppendChild(itemsResolved);
    foreach (XmlNode newResolvedItem in newResolvedItems)
    {
      itemsResolved.AppendChild(newResolvedItem);
    }
    return itemsResolved.ChildNodes;
  }

  protected virtual XmlNode GetTemplateByXPath(XmlNode templatesBaseNode, string xpath)
  {
    return templatesBaseNode.SelectSingleNode(xpath);
  }

  protected virtual List<XmlNode> ResolveContentNodesFromTemplate(XmlNode contentNode)
  {
    var result = new List<XmlNode>();
    XmlNode itemsNode = contentNode.SelectSingleNode("items");
    XmlNode templatesParentNode = contentNode.SelectSingleNode("templates");
    Debug.Assert(itemsNode != null && templatesParentNode != null);
    if (itemsNode.ChildNodes.Count == 0 || templatesParentNode.ChildNodes.Count == 0)
    {
      return result;
    }
    string defaultTemplateXPath = XmlHelper.GetAttributeValue(itemsNode, "templateXPath");
    if (defaultTemplateXPath.IsNullOrEmpty())
    {
      defaultTemplateXPath = "*";
    }
    foreach (XmlNode itemNode in itemsNode.ChildNodes)
    {
      string itemTemplateXPath = XmlHelper.GetAttributeValue(itemNode, "templateXPath");
      if (itemTemplateXPath.IsNullOrEmpty())
      {
        itemTemplateXPath = defaultTemplateXPath;
      }
      XmlNode currentTemplateNode = this.GetTemplateByXPath(templatesParentNode, itemTemplateXPath);
      var keyValuePairs = this.ExtractAllKeyValues(itemNode);
      var resultItemNode = this.GetItemNodeFromTemplate(currentTemplateNode, keyValuePairs, itemNode);
      if (resultItemNode != null)
      {
        result.Add(resultItemNode);
      }
    }
    return result;
  }

  protected virtual void SubstituteVariablesInItemNode(
    XmlNode itemNode,
    Dictionary<string, string> variableValues,
    XmlNode originalItemNode)
  {
    /*if (itemNode.Name.Equals("templates", StringComparison.OrdinalIgnoreCase))
              return;*/
    if (this.CheckAndInsertSubtree(itemNode, originalItemNode))
    {
      return;
    }
    var varName = this.ExtractVariableNameFromNodeValueStrong(itemNode);
    if (varName != null && variableValues.ContainsKey(varName))
    {
      itemNode.Value = variableValues[varName];
    }
    if (itemNode.Attributes != null)
    {
      foreach (XmlAttribute xmlAttribute in itemNode.Attributes)
      {
        var variableName = this.ExtractVariableNameFromNodeValueSoft(xmlAttribute);
        if (variableName.IsNullOrEmpty())
        {
          continue;
        }
        if (variableValues.ContainsKey(variableName))
        {
          xmlAttribute.Value = xmlAttribute.Value.Replace("{" + variableName + "}", variableValues[variableName]);
        }
      }
    }
    foreach (XmlNode childNode in itemNode.ChildNodes)
    {
      this.SubstituteVariablesInItemNode(childNode, variableValues, originalItemNode);
    }
  }

  protected virtual bool SupportTemplating(XmlNode contentNode)
  {
    XmlNode itemsNode = contentNode.SelectSingleNode("items");
    XmlNode templateNode = contentNode.SelectSingleNode("templates");
    return itemsNode != null && templateNode != null;
  }
}