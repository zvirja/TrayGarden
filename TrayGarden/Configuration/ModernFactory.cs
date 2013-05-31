using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.Annotations;
using TrayGarden.Configuration.ModernFactoryStuff;
using TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners;
using TrayGarden.Configuration.ModernFactoryStuff.Parcers;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Resources;

namespace TrayGarden.Configuration
{
    public class ModernFactory : IFactory
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
            SubstituteReferences(xmlConfiguration, xmlConfiguration);
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

        protected static void SubstituteReferences(XmlNode currentNode, XmlNode allConnfiguration)
        {
            if (currentNode.Attributes != null && currentNode.Attributes["ref"] != null && currentNode.Attributes["ref"].Value.NotNullNotEmpty())
            {
                var xpath = currentNode.Attributes["ref"].Value;
                var sourceNode = XmlHelper.SmartlySelectSingleNode(allConnfiguration, xpath);
                if (sourceNode != null)
                {
                    currentNode.ParentNode.ReplaceChild(sourceNode.Clone(), currentNode);
                }
            }
            else
            {
                foreach (XmlNode innerNode in currentNode.ChildNodes)
                {
                    SubstituteReferences(innerNode, allConnfiguration);
                }
            }
        }


        protected static XmlDocument ResolveConfiguration()
        {
            var mainSection = ConfigurationManager.GetSection("trayGarden") as SectionHandler;
            if (mainSection != null)
            {
                Log.Info("ModernFactory from AppConfig resolved", typeof(ModernFactory));
                return mainSection.XmlRepresentation;
            }
            var embeddedConfiguration = GetEmbeddedConfiguration();
            Log.Info("ModernFactory from AppConfig resolved", typeof(ModernFactory));
            return embeddedConfiguration;
        }

        protected static XmlDocument GetEmbeddedConfiguration()
        {
            var document = new XmlDocument();
            string embeddedConfigValue = GlobalResourcesManager.GetStringByName("XmlConfiguration");
            Assert.IsNotNullOrEmpty(embeddedConfigValue, "Embedded configuration is empty");
            document.LoadXml(embeddedConfigValue);
            return document;
        }

        protected static ModernFactory GetModernFactoryInstance(XmlDocument document, string nodePath)
        {
            XmlNode factoryNode = document.SelectSingleNode(nodePath);
            Assert.IsNotNull(factoryNode, "Factory node ({0}) in configuration wasn't found".FormatWith(nodePath));
            var type = factoryNode.Attributes["type"].Value;
            var instance = (ModernFactory)Activator.CreateInstance(Type.GetType(type));
            return instance;
        }

        #endregion

        #region Public methods

        public ModernFactory()
        {
            ObjectInfosCache = new Dictionary<object, ObjectInfo>();
            ParcerResolver = new ParcerResolver(this);
            ContentAssignersResolver = new ContentAssignersResolver();
            Log.Debug("Modern factory object created", this);
        }

        public virtual object GetObject([NotNull] string configurationPath)
        {
            Assert.ArgumentNotNull(configurationPath, "configurationPath");
            return GetObjectFromPathInternal(configurationPath, true);
        }

        public virtual object GetObject([NotNull] XmlNode configurationNode)
        {
            Assert.ArgumentNotNull(configurationNode, "configurationNode");
            return GetObjectFromNodeInternal(configurationNode, true);
        }

        public virtual object GetPurelyNewObject([NotNull] string configurationPath)
        {
            Assert.ArgumentNotNull(configurationPath, "configurationPath");
            return GetObjectFromPathInternal(configurationPath, false);
        }

        public virtual T GetObject<T>([NotNull] string configurationPath) where T : class
        {
            Assert.ArgumentNotNull(configurationPath, "configurationPath");
            var result = GetObject(configurationPath);
            var castedResult = result as T;
            if (result != null && castedResult == null)
                Log.Warn(
                    "GetObject(). Type, specified in config differs from required type. path:{0}, specified type:{1}, required type:{2}"
                        .FormatWith(configurationPath, result.GetType().FullName, typeof(T).FullName), this);
            return castedResult;
        }

        public virtual T GetPurelyNewObject<T>([NotNull] string configurationPath) where T : class
        {
            Assert.ArgumentNotNull(configurationPath, "configurationPath");
            var result = GetPurelyNewObject(configurationPath);
            var castedResult = result as T;
            if (result != null && castedResult == null)
                Log.Warn(
                    "GetPurelyNewObject(). Type, specified in config differs from required type. path:{0}, specified type:{1}, required type:{2}"
                        .FormatWith(configurationPath, result.GetType().FullName, typeof(T).FullName), this);
            return castedResult;
        }

        /*public virtual T GetObject<T>(XmlNode configurationNode) where T : class
        {
            return GetObject(configurationNode) as T;
        }*/

        public virtual string GetStringSetting([NotNull] string settingName, string defaultValue)
        {
            Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
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

        #region ObjectInfo abstraction
        protected virtual object GetObjectFromPathInternal(string configurationPath, bool allowSingleton)
        {
            if (ObjectInfosCache.ContainsKey(configurationPath))
                return GetObjectFromObjectInfo(ObjectInfosCache[configurationPath], allowSingleton);
            var configurationNode = XmlHelper.SmartlySelectSingleNode(XmlConfiguration, configurationPath);
            var newObjectInfo = GetObjectInfoFromNode(configurationNode);
            ObjectInfosCache[configurationPath] = newObjectInfo;
            return GetObjectFromObjectInfo(newObjectInfo, allowSingleton);
        }

        protected virtual object GetObjectFromNodeInternal(XmlNode configurationNode, bool allowSingleton)
        {
            var newObjectInfo = GetObjectInfoFromNode(configurationNode);
            return GetObjectFromObjectInfo(newObjectInfo, allowSingleton);
        }

        protected virtual ObjectInfo GetObjectInfoFromNode(XmlNode configurationNode)
        {
            if (configurationNode == null)
                return null;
            if (ObjectInfosCache.ContainsKey(configurationNode))
                return ObjectInfosCache[configurationNode];
            bool makeSingleton;
            object instance = CreateInstanceInternal(configurationNode, out makeSingleton);
            if (instance == null)
            {
                ObjectInfosCache[configurationNode] = null;
                return null;
            }
            var isPrototype = instance is ISupportPrototyping;
            var isSingleton = makeSingleton || XmlHelper.GetAttributeValue(configurationNode, "singleton").ToUpperInvariant().Equals("TRUE", StringComparison.OrdinalIgnoreCase);
            var objectInfo = new ObjectInfo(instance, configurationNode, isSingleton, isPrototype);
            ObjectInfosCache[configurationNode] = objectInfo;
            return objectInfo;
        }

        protected virtual object GetObjectFromObjectInfo(ObjectInfo objectInfo, bool allowSingleton)
        {
            if (objectInfo == null)
                return null;
            if (objectInfo.IsSingleton)
                return allowSingleton ? objectInfo.Instance : null;
            if (objectInfo.IsPrototype)
                return ((ISupportPrototyping)objectInfo.Instance).CreateNewInializedInstance();
            //The first instance is always assigned. So we can use it for one time.
            if (objectInfo.Instance != null)
            {
                object instance = objectInfo.Instance;
                objectInfo.Instance = null;
                return instance;
            }
            bool makeSingleton;
            return CreateInstanceInternal(objectInfo.ConfigurationNode, out makeSingleton);
        }

        #endregion

        #region Object instance creation & content assigning
        protected virtual object CreateInstanceInternal(XmlNode configurationNode, out bool makeSingleton)
        {
            makeSingleton = false;
            try
            {
                if (configurationNode == null)
                    return null;
                var specialInstance = CreateSpecialObject(configurationNode, out makeSingleton);
                if (specialInstance != null)
                    return specialInstance;
                string typeStrValue = XmlHelper.GetAttributeValue(configurationNode, "type");
                Assert.IsNotNullOrEmpty(typeStrValue, "Type shouldn't be null");
                Type typeObj = ReflectionHelper.ResolveType(typeStrValue);
                Assert.IsNotNull(typeObj, "Type is invalid");
                object instance = Activator.CreateInstance(typeObj);
                AssignContent(configurationNode, instance);
                return instance;
            }
            catch (Exception ex)
            {
                Log.Error(
                    "Expception during instance creation. ConfigurationNodeName: {0}".FormatWith(configurationNode.Name),
                    this, ex);
                return null;
            }
        }

        protected virtual void AssignContent(XmlNode configurationNode, object instance)
        {
            Type instanceType = instance.GetType();
            foreach (XmlNode contentNode in configurationNode.ChildNodes)
            {
                try
                {
                    var hint = GetHintValue(contentNode);
                    var contentAssigner = ResolvePropertyContentAssigner(hint);
                    contentAssigner.AssignContent(contentNode, instance, instanceType, GetValueParcer);
                }
                catch
                {
                    Log.Warn(
                        "Unable to assign content node. instance type:{0}, node name:{1}".FormatWith(
                            instance.GetType().FullName, contentNode.Name), this);
                }
            }
        }

        protected virtual string GetHintValue(XmlNode configurationNode)
        {
            return XmlHelper.GetAttributeValue(configurationNode, "hint");
        }
        #endregion

        #region Value parcers and content assigners
        private IParcer GetValueParcer(Type type)
        {
            return ParcerResolver.GetParcer(type);
        }

        protected virtual IContentAssigner ResolvePropertyContentAssigner(string hint)
        {
            return ContentAssignersResolver.GetPropertyAssigner(hint);
        }


        protected virtual IContentAssigner ResolveDirectContentAssigner(string hint)
        {
            return ContentAssignersResolver.GetDirectAssigner(hint);
        }

        #endregion

        #region Special object functionality
        protected virtual object CreateSpecialObject(XmlNode objectConfigurationNode, out bool makeSingleton)
        {
            makeSingleton = false;
            if (!IsSpecialObject(objectConfigurationNode))
                return null;
            var typeAttribValue = XmlHelper.GetAttributeValue(objectConfigurationNode, "type");
            var specialPrefix = typeAttribValue.Substring(0,
                                                          typeAttribValue.IndexOf(":",
                                                                                  StringComparison.OrdinalIgnoreCase));
            object objectInstance = CreateSpecialObjectInstance(objectConfigurationNode, specialPrefix, out makeSingleton);
            if (objectInstance == null)
                return null;
            var contentAssigner = ResolveDirectContentAssigner(specialPrefix);
            if (contentAssigner != null)
                contentAssigner.AssignContent(objectConfigurationNode, objectInstance, objectInstance.GetType(), GetValueParcer);
            return objectInstance;
        }

        protected virtual bool IsSpecialObject(XmlNode objectConfigurationNode)
        {
            var type = XmlHelper.GetAttributeValue(objectConfigurationNode, "type");
            if (type.Contains(":"))
                return true;
            return false;
        }

        protected virtual object CreateSpecialObjectInstance(XmlNode objectConfigurationNode, string specialPrefix, out bool makeSingleton)
        {
            makeSingleton = false;
            if (specialPrefix.ToUpperInvariant().Equals("NEWLIST"))
                return CreateSpecialNewList(objectConfigurationNode, out makeSingleton);
            if (specialPrefix.ToUpperInvariant().Equals("OBJECTFACTORY"))
                return CreateSpecialObjectFactory(objectConfigurationNode, out makeSingleton);
            Log.Warn("Unknown special prefix: {0}".FormatWith(specialPrefix), this);
            return null;
        }

        protected virtual object CreateSpecialNewList(XmlNode objectConfigurationNode, out bool makeSingleton)
        {
            makeSingleton = false;
            string typeStr = XmlHelper.GetAttributeValue(objectConfigurationNode, "type");
            typeStr = typeStr.Substring(typeStr.IndexOf(":", StringComparison.OrdinalIgnoreCase) + 1);
            Type type = ReflectionHelper.ResolveType(typeStr);
            if (type == null)
                return null;
            var listGeneric = typeof(List<>).MakeGenericType(new Type[] { type });
            return Activator.CreateInstance(listGeneric);
        }

        protected virtual object CreateSpecialObjectFactory(XmlNode objectConfigurationNode, out bool makeSingleton)
        {
            makeSingleton = true;
            return new ObjectFactory(this);
        }

        #endregion

        #region Settings
        protected virtual void InitializeSettings()
        {
            Settings = new Dictionary<string, string>();
            var settingsParentNode = XmlHelper.SmartlySelectSingleNode(XmlConfiguration, SettingsNodePath);
            if (settingsParentNode == null)
            {
                Log.Warn("Unable to find settings node. Node path:{0}".FormatWith(SettingsNodePath), this);
                return;
            }
            XmlNodeList settingNodes = settingsParentNode.ChildNodes;
            foreach (XmlNode settingNode in settingNodes)
            {
                var name = XmlHelper.GetAttributeValue(settingNode, "name");
                var value = XmlHelper.GetAttributeValue(settingNode, "value");
                if (name.NotNullNotEmpty() && value.NotNullNotEmpty())
                    Settings[name] = value;
            }
        }

        #endregion

        #region Object Factory class
        public class ObjectFactory : IObjectFactory
        {
            protected ObjectInfo ObjectInfo { get; set; }
            protected ModernFactory ModernFactoryInstance { get; set; }

            public ObjectFactory(ModernFactory modernFactory)
            {
                ModernFactoryInstance = modernFactory;
            }

            public void Initialize(XmlNode instanceConfigurationNode)
            {
                ObjectInfo = ModernFactoryInstance.GetObjectInfoFromNode(instanceConfigurationNode);
            }

            public virtual object GetObject()
            {
                return ModernFactoryInstance.GetObjectFromObjectInfo(ObjectInfo, true);
            }
            public virtual object GetPurelyNewObject()
            {
                return ModernFactoryInstance.GetObjectFromObjectInfo(ObjectInfo, false);
            }
        }

        #endregion

    }
}