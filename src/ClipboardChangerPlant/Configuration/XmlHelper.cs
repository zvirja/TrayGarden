using System.Xml;

namespace ClipboardChangerPlant.Configuration;

public class XmlHelper
{
  public XmlHelper(XmlNode parentNode)
  {
    ParentNode = parentNode;
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

  public static bool GetBoolValue(XmlNode parent, string nodePath, bool defaultValue)
  {
    var strValue = GetStringValue(parent, nodePath, null);
    if (strValue == null)
    {
      return defaultValue;
    }
    bool result;
    if (bool.TryParse(strValue, out result))
    {
      return result;
    }
    return defaultValue;
  }

  public static int GetIntValue(XmlNode parent, string nodePath, int defaultValue)
  {
    var strValue = GetStringValue(parent, nodePath, null);
    if (strValue == null)
    {
      return defaultValue;
    }
    int result;
    if (int.TryParse(strValue, out result))
    {
      return result;
    }
    return defaultValue;
  }

  public static string GetStringValue(XmlNode parent, string nodePath, string defaultValue)
  {
    var innerNode = SmartSelectSingleNode(parent, nodePath);
    return innerNode == null ? defaultValue : innerNode.InnerText;
  }

  public static XmlNodeList SmartSelectNodes(XmlNode parent, string nodePath)
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

  public static XmlNode SmartSelectSingleNode(XmlNode parent, string nodePath)
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

  public bool GetBoolValue(string nodePath, bool defaultValue)
  {
    return GetBoolValue(ParentNode, nodePath, defaultValue);
  }

  public int GetIntValue(string nodePath, int defaultValue)
  {
    return GetIntValue(ParentNode, nodePath, defaultValue);
  }

  public string GetStringValue(string nodePath, string defaultValue)
  {
    return GetStringValue(ParentNode, nodePath, defaultValue);
  }
}