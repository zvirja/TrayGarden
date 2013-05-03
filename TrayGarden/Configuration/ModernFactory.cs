using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using TrayGarden.Configuration.ModernFactoryStuff;
using TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners;
using TrayGarden.Configuration.ModernFactoryStuff.Parcers;
using TrayGarden.Helpers;
using TrayGarden.Resources;

namespace TrayGarden.Configuration
{
    public class ModernFactory: IFactory
    {
        protected static ModernFactory _actualFactory;
        protected static object _lock = new object();
        protected const string SettingsNodePath = "trayGarden/settings";
        protected Dictionary<string, string> Settings;
        protected Dictionary<object, ObjectInfo> ObjectInfosCache { get; set; }
        protected ParcerResolver ParcerResolver { get; set; }
        protected ContentAssignersResolver ContentAssignersResolver { get; set; }

        public XmlDocument XmlConfiguration { get; protected set; }

        public static ModernFactory ActualInstance
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

        #region Public methods

        public ModernFactory()
        {
            ObjectInfosCache = new Dictionary<object, ObjectInfo>();
            ParcerResolver = new ParcerResolver(this);
            ContentAssignersResolver = new ContentAssignersResolver();
        }

        public virtual object GetObject(string configurationPath)
        {
            return GetObjectFromPathInternal(configurationPath, true);
        }

        public virtual object GetObject(XmlNode configurationNode)
        {
            return GetObjectFromNodeInternal(configurationNode, true);
        }

        public virtual object GetPurelyNewObject(string configurationPath)
        {
            return GetObjectFromPathInternal(configurationPath, false);
        }

        public virtual T GetObject<T>(string configurationPath) where T : class
        {
            return GetObject(configurationPath) as T;
        }

        public virtual T GetPurelyNewObject<T>(string configurationPath) where T : class
        {
            return GetPurelyNewObject(configurationPath) as T;
        }

        /*public virtual T GetObject<T>(XmlNode configurationNode) where T : class
        {
            return GetObject(configurationNode) as T;
        }*/

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

        #endregion


        protected virtual object GetObjectFromPathInternal(string configurationPath, bool allowSingletone)
        {
            if (ObjectInfosCache.ContainsKey(configurationPath))
                return GetObjectFromObjectInfo(ObjectInfosCache[configurationPath], allowSingletone);
            var configurationNode = XmlHelper.SmartlySelectSingleNode(XmlConfiguration, configurationPath);
            var newObjectInfo = GetObjectInfoFromNode(configurationNode);
            ObjectInfosCache[configurationPath] = newObjectInfo;
            return GetObjectFromObjectInfo(newObjectInfo, allowSingletone);
        }

        protected virtual object GetObjectFromNodeInternal(XmlNode configurationNode, bool allowSingletone)
        {
            var newObjectInfo = GetObjectInfoFromNode(configurationNode);
            return GetObjectFromObjectInfo(newObjectInfo, allowSingletone);
        }

        protected virtual object CreateInstanceInternal(XmlNode configurationNode)
        {
            try
            {
                if (configurationNode == null)
                    return null;
                string typeStrValue = GetAttributeValue(configurationNode, "type");
                if (typeStrValue.IsNullOrEmpty())
                    return null;
                Type typeObj = ReflectionHelper.ResolveType(typeStrValue);
                if (typeObj == null)
                    return null;
                object instance = Activator.CreateInstance(typeObj);
                AssignContent(configurationNode, instance);
                return instance;
            }
            catch
            {
                return null;
            }
        }

        protected virtual ObjectInfo GetObjectInfoFromNode(XmlNode configurationNode)
        {
            if (configurationNode == null)
                return null;
            if (ObjectInfosCache.ContainsKey(configurationNode))
                return ObjectInfosCache[configurationNode];
            object instance = CreateInstanceInternal(configurationNode);
            if (instance == null)
            {
                ObjectInfosCache[configurationNode] = null;
                return null;
            }
            var isPrototype = instance is IPrototype;
            var isSingletone = GetAttributeValue(configurationNode, "singletone").ToUpperInvariant().Equals("TRUE", StringComparison.OrdinalIgnoreCase);
            var objectInfo = new ObjectInfo(instance, configurationNode, isSingletone, isPrototype);
            ObjectInfosCache[configurationNode] = objectInfo;
            return objectInfo;
        }

        protected virtual object GetObjectFromObjectInfo(ObjectInfo objectInfo, bool allowSingletone)
        {
            if (objectInfo == null)
                return null;
            if (objectInfo.IsSingletone)
                return allowSingletone ? objectInfo.Instance : null;
            if (objectInfo.IsPrototype)
                return ((IPrototype)objectInfo.Instance).CreateNewInializedInstance();
            //The first instance is always assigned. So we can use it for one time.
            if (objectInfo.Instance != null)
            {
                object instance = objectInfo.Instance;
                objectInfo.Instance = null;
                return instance;
            }
            return CreateInstanceInternal(objectInfo.ConfigurationNode);
        }

        protected virtual string GetAttributeValue(XmlNode node, string attributeName)
        {
            if (node == null || node.Attributes == null)
                return string.Empty;
            var attribute = node.Attributes[attributeName];
            if (attribute == null)
                return string.Empty;
            return attribute.Value;
        }

        protected virtual string GetHintValue(XmlNode configurationNode)
        {
            return GetAttributeValue(configurationNode, "hint");
        }

        protected virtual void AssignContent(XmlNode configurationNode, object instance)
        {
            Type instanceType = instance.GetType();
            foreach (XmlNode contentNode in configurationNode.ChildNodes)
            {
                var hint = GetHintValue(contentNode);
                var contentAssigner = ResolveContentAssigner(hint);
                contentAssigner.AssignContent(contentNode, instance, instanceType, GetValueParcer);
            }
        }

        private IParcer GetValueParcer(Type type)
        {
            return ParcerResolver.GetParcer(type);
        }

        protected virtual IContentAssigner ResolveContentAssigner(string hint)
        {
            return ContentAssignersResolver.GetAssigner(hint);
        }

        protected virtual void InitializeSettings()
        {
            Settings = new Dictionary<string, string>();
            var settingsParentNode = XmlHelper.SmartlySelectSingleNode(XmlConfiguration, SettingsNodePath);
            if (settingsParentNode == null)
                return;
            XmlNodeList settingNodes = settingsParentNode.ChildNodes;
            foreach (XmlNode settingNode in settingNodes)
            {
                var name = GetAttributeValue(settingNode, "name");
                var value = GetAttributeValue(settingNode, "value");
                if (name.NotNullNotEmpty() && value.NotNullNotEmpty())
                    Settings[name] = value;
            }
        }
    }
}