using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TrayGarden.Helpers
{
    public class XmlHelper
    {
        #region Static helpers

        public static XmlNode SmartlySelectSingleNode(XmlNode parent, string nodePath)
        {
            var innerNode = parent.SelectSingleNode(nodePath);
            if (innerNode != null)
                return innerNode;
            nodePath = FixNodePath(parent, nodePath);
            innerNode = parent.SelectSingleNode(nodePath);
            return innerNode;
        }

        public static XmlNodeList SmartlySelectNodes(XmlNode parent, string nodePath)
        {
            var innerNodes = parent.SelectNodes(nodePath);
            if (innerNodes.Count > 0)
                return innerNodes;
            nodePath = FixNodePath(parent, nodePath);
            innerNodes = parent.SelectNodes(nodePath);
            return innerNodes;
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

        public static string GetAttributeValue(XmlNode node, string attributeName)
        {
            if (node == null || node.Attributes == null)
                return string.Empty;
            var attribute = node.Attributes[attributeName];
            if (attribute == null)
                return string.Empty;
            return attribute.Value;
        }

        #endregion

        #region Instance helpers

        public XmlNode ParentNode { get; set; }

        public XmlHelper(XmlNode parentNode)
        {
            ParentNode = parentNode;
        }

        public virtual string GetStringValue(string nodePath)
        {
            return GetStringValue(ParentNode, nodePath);
        }

        public virtual List<string> GetStringsList(string nodePath)
        {
            return GetStringsList(ParentNode, nodePath);
        }

        public virtual bool GetBoolValue(string nodePath)
        {
            return GetBoolValue(ParentNode, nodePath);
        }

        public virtual int GetIntValue(string nodePath)
        {
            return GetIntValue(ParentNode, nodePath);
        }

        public virtual string GetName()
        {
            return GetStringValue("name");
        }

        public virtual XmlNodeList SmartlySelectNodes(string xpath)
        {
            return SmartlySelectNodes(ParentNode, xpath);
        }

        public virtual XmlNode SmartlySelectSingleNode(string xpath)
        {
            return SmartlySelectSingleNode(ParentNode, xpath);
        }

        public virtual string GetAttributeValue(string attributeName)
        {
            return GetAttributeValue(ParentNode, attributeName);
        }

        #endregion
    }
}