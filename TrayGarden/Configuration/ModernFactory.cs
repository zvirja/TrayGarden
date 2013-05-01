using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using TrayGarden.Configuration.ModernFactoryStuff;
using TrayGarden.Helpers;
using TrayGarden.Resources;

namespace TrayGarden.Configuration
{
    public class ModernFactory
    {
        protected static ModernFactory _actualFactory;
        protected static object _lock = new object();
        protected const string SettingsNodePath = "trayGarden/settings";
        protected Dictionary<string, string> Settings;

        public XmlDocument XmlConfiguration { get; protected set; }

        public static ModernFactory Instance
        {
            get
            {
                if (_actualFactory != null)
                    return _actualFactory;
                lock (_lock)
                {
                    if (_actualFactory != null)
                        return _actualFactory;
                    _actualFactory = ResolveModernFactory();
                    return _actualFactory;
                }
            }
        }

        #region Protected static

        protected static ModernFactory ResolveModernFactory()
        {
            var xmlConfiguration = ResolveConfiguration();
            var factoryInstance = GetModernFactoryInstance(xmlConfiguration, "trayGarden/modernFactory");
            CleanupXmlNodeTree(xmlConfiguration);
            factoryInstance.XmlConfiguration = xmlConfiguration;
            return factoryInstance;
        }

        protected static void CleanupXmlNodeTree(XmlNode node)
        {
            /* var nodesToRemove = new List<XmlNode>();
            var childNodes = node.ChildNodes;
            foreach (XmlNode innerNode in childNodes)
            {
                if (innerNode.NodeType == XmlNodeType.Comment || innerNode.NodeType == XmlNodeType.CDATA)
                    nodesToRemove.Add(innerNode);
                else
                    CleanupXmlNodeTree(node);
            }
            foreach (XmlNode xmlNode in nodesToRemove)
            {
                node.RemoveChild(xmlNode);
            }*/

            var childNodes = node.ChildNodes;
            for (int i = childNodes.Count - 1; i >= 0; i--)
            {
                XmlNode innerNode = childNodes[i];
                if (innerNode.NodeType == XmlNodeType.Comment || innerNode.NodeType == XmlNodeType.CDATA)
                    node.RemoveChild(innerNode);
                else
                    CleanupXmlNodeTree(innerNode);
            }
        }


        protected static XmlDocument ResolveConfiguration()
        {
            var mainSection = ConfigurationManager.GetSection("trayGarden") as SectionHandler;
            if (mainSection != null)
                return mainSection.XmlRepresentation;
            var embeddedConfiguration = GetEmbeddedConfiguration();
            return embeddedConfiguration;
        }

        protected static XmlDocument GetEmbeddedConfiguration()
        {
            var document = new XmlDocument();
            document.LoadXml(GlobalResourcesManager.GetStringByName("XmlConfiguration"));
            return document;
        }

        protected static ModernFactory GetModernFactoryInstance(XmlDocument document, string nodePath)
        {
            XmlNode factoryNode = document.SelectSingleNode(nodePath);
            var type = factoryNode.Attributes["type"].Value;
            var instance = (ModernFactory) Activator.CreateInstance(Type.GetType(type));
            return instance;
        }

        #endregion

        public virtual object GetObject(string configurationPath)
        {
            var configurationNode = XmlHelper.SmartlySelectSingleNode(XmlConfiguration, configurationPath);
            return GetInstanceInternal(configurationNode);
        }

        public virtual object GetObject(XmlNode configurationNode)
        {
            return GetInstanceInternal(configurationNode);
        }

        public virtual T GetObject<T>(string configurationPath) where T : class
        {
            return GetObject(configurationPath) as T;
        }

        public virtual T GetObject<T>(XmlNode configurationNode) where T : class
        {
            return GetObject(configurationNode) as T;
        }

        public virtual string GetStringSetting(string settingName, string defaultValue)
        {
            if (Settings == null)
            {
                lock (_lock)
                {
                    if (Settings == null)
                        InitializeSettings();
                }
            }
            if (Settings.ContainsKey(settingName))
                return Settings[settingName];
            return defaultValue;
        }

        protected virtual object GetInstanceInternal(XmlNode configuraionNode)
        {
            if (configuraionNode == null)
                return null;
            string hint = getHintValue(configuraionNode);
            if (!hint.IsNullOrEmpty() && hint.Equals("skip", StringComparison.OrdinalIgnoreCase))
                return null;
            XmlAttribute typeAttribute = configuraionNode.Attributes["type"];
            if (typeAttribute == null)
                return null;
            string typeStrValue = typeAttribute.Value;
            if (typeStrValue.IsNullOrEmpty())
                return null;
            Type typeObj = ReflectionHelper.ResolveType(typeStrValue);
            if (typeObj == null)
                return null;
            object instance = Activator.CreateInstance(typeObj);
            AssignContent(configuraionNode, instance);
            var needInitializationInstance = instance as IRequireInitialization;
            if (needInitializationInstance != null)
                needInitializationInstance.Initialize();
            return instance;
        }

        protected virtual string getHintValue(XmlNode configurationNode)
        {
            if (configurationNode == null || configurationNode.Attributes == null)
                return string.Empty;
            var hintAttribute = configurationNode.Attributes["hint"];
            if (hintAttribute == null)
                return string.Empty;
            return hintAttribute.Value;
        }

        protected virtual void AssignContent(XmlNode configurationNode, object instance)
        {
            Type instanceType = instance.GetType();
            foreach (XmlNode contentNode in configurationNode.ChildNodes)
            {
                var hint = getHintValue(contentNode);
                if (hint.Equals("skip", StringComparison.OrdinalIgnoreCase))
                    continue;
                var contentAssigner = ResolveContentAssigner(hint);
                contentAssigner.AssignContent(contentNode, instance, instanceType, ValueParcerResolver);
            }
        }

        private IParcer ValueParcerResolver(Type type)
        {
            if (type == typeof (string))
                return StringParcer.Instance;
            if (type == typeof (bool))
                return BoolParcer.Instance;
            if (type == typeof (int))
                return IntParcer.Instance;
            return ObjectParcer.Instance;
        }

        protected virtual IContentAssigner ResolveContentAssigner(string hint)
        {
            switch (hint)
            {
                case "raw":
                    return MethodAssigner.Instance;
                case "list":
                    return ListAssigner.Instance;
                default:
                    return SimpleAssigner.Instance;
            }
        }

        protected virtual void InitializeSettings()
        {
            Settings = new Dictionary<string, string>();
            var settingsParentNode = XmlHelper.SmartlySelectSingleNode(XmlConfiguration, SettingsNodePath);
            if (settingsParentNode == null)
                return;
            XmlNodeList settingNodes = settingsParentNode.ChildNodes;
            if (settingNodes.Count == 0)
                return;
            foreach (XmlNode settingNode in settingNodes)
            {
                var nameAttribute = settingNode.Attributes["name"];
                var valueAttribute = settingNode.Attributes["value"];
                if (nameAttribute == null || valueAttribute == null)
                    continue;
                Settings[nameAttribute.Value] = valueAttribute.Value;
            }
        }
    }
}