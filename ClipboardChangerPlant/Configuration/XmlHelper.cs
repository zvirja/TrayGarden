using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ClipboardChangerPlant.Configuration
{
  public class XmlHelper
  {
    public static string GetStringValue(XmlNode parent, string nodePath)
    {
      var innerNode = SmartSelectSingleNode(parent, nodePath);
      return innerNode.InnerText;
    }

    public static XmlNode SmartSelectSingleNode(XmlNode parent, string nodePath)
    {
      var innerNode = parent.SelectSingleNode(nodePath);
      if (innerNode != null)
        return innerNode;
      nodePath = FixNodePath(parent, nodePath);
      innerNode = parent.SelectSingleNode(nodePath);
      return innerNode;
    }

    public static XmlNodeList SmartSelectNodes(XmlNode parent, string nodePath)
    {
      var innerNodes = parent.SelectNodes(nodePath);
      if (innerNodes.Count > 0)
        return innerNodes;
      nodePath = FixNodePath(parent, nodePath);
      innerNodes = parent.SelectNodes(nodePath);
      return innerNodes;
    }

    public static bool GetBoolValue(XmlNode node, string nodePath)
    {
      return bool.Parse(GetStringValue(node, nodePath));
    }

    public static int GetIntValue(XmlNode parent, string nodePath)
    {
      return int.Parse(GetStringValue(parent, nodePath));
    }

    public static string FixNodePath(XmlNode parent, string nodePath)
    {
      var asDocument = parent as XmlDocument;
      string prefix = asDocument != null ? asDocument.FirstChild.Name : parent.Name;
      if (nodePath.StartsWith(prefix))
        return nodePath;
      return prefix + "/" + nodePath;
    }

    public XmlNode ParentNode { get; set; }

    public XmlHelper(XmlNode parentNode)
    {
      ParentNode = parentNode;
    }

    public string GetStringValue(string nodePath)
    {
      return GetStringValue(ParentNode, nodePath);
    }

    public bool GetBoolValue(string nodePath)
    {
      return GetBoolValue(ParentNode, nodePath);
    }

    public int GetIntValue(string nodePath)
    {
      return GetIntValue(ParentNode, nodePath);
    }
  }
}
