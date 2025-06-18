using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TrayGarden.Helpers;

public class XmlHelper
{
  public XmlHelper(XmlNode parentNode)
  {
    this.ParentNode = parentNode;
  }

  public XmlNode ParentNode { get; set; }

  public static string FixNodePath(XmlNode parent, string nodePath)
  {
    var asDocument = parent as XmlDocument;
    string prefix = asDocument != null ? asDocument.FirstChild.Name : parent.Name;
    if (nodePath.StartsWith(prefix))
    {
      return nodePath;
    }
    return prefix + "/" + nodePath;
  }

  public static string GetAttributeValue(XmlNode node, string attributeName)
  {
    if (node == null || node.Attributes == null)
    {
      return string.Empty;
    }
    var attribute = node.Attributes[attributeName];
    if (attribute == null)
    {
      return string.Empty;
    }
    return attribute.Value;
  }

  public static bool GetBoolValue(XmlNode node, string nodePath)
  {
    return bool.Parse(GetStringValue(node, nodePath));
  }

  public static int GetIntValue(XmlNode parent, string nodePath)
  {
    return int.Parse(GetStringValue(parent, nodePath));
  }

  public static string GetStringValue(XmlNode parent, string nodePath)
  {
    var innerNode = SmartlySelectSingleNode(parent, nodePath);
    return innerNode.InnerText;
  }

  public static List<string> GetStringsList(XmlNode parent, string xPath)
  {
    XmlNodeList innerNode = SmartlySelectNodes(parent, xPath);
    var result = (from XmlNode node in innerNode select node.InnerText).ToList();
    return result;
  }

  public static XmlNodeList SmartlySelectNodes(XmlNode parent, string nodePath)
  {
    var innerNodes = parent.SelectNodes(nodePath);
    if (innerNodes.Count > 0)
    {
      return innerNodes;
    }
    nodePath = FixNodePath(parent, nodePath);
    innerNodes = parent.SelectNodes(nodePath);
    return innerNodes;
  }

  public static XmlNode SmartlySelectSingleNode(XmlNode parent, string nodePath)
  {
    var innerNode = parent.SelectSingleNode(nodePath);
    if (innerNode != null)
    {
      return innerNode;
    }
    nodePath = FixNodePath(parent, nodePath);
    innerNode = parent.SelectSingleNode(nodePath);
    return innerNode;
  }

  public virtual string GetAttributeValue(string attributeName)
  {
    return GetAttributeValue(this.ParentNode, attributeName);
  }

  public virtual bool GetBoolValue(string nodePath)
  {
    return GetBoolValue(this.ParentNode, nodePath);
  }

  public virtual int GetIntValue(string nodePath)
  {
    return GetIntValue(this.ParentNode, nodePath);
  }

  public virtual string GetName()
  {
    return this.GetStringValue("name");
  }

  public virtual string GetStringValue(string nodePath)
  {
    return GetStringValue(this.ParentNode, nodePath);
  }

  public virtual List<string> GetStringsList(string nodePath)
  {
    return GetStringsList(this.ParentNode, nodePath);
  }

  public virtual XmlNodeList SmartlySelectNodes(string xpath)
  {
    return SmartlySelectNodes(this.ParentNode, xpath);
  }

  public virtual XmlNode SmartlySelectSingleNode(string xpath)
  {
    return SmartlySelectSingleNode(this.ParentNode, xpath);
  }
}