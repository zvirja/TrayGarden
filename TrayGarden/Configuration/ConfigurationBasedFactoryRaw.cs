using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using TrayGarden.Helpers;
using TrayGarden.Resources;

namespace TrayGarden.Configuration
{
    public class ConfigurationBasedFactoryRaw
    {
        protected XmlDocument _mainConfigurationNode;
        protected const string SettingsNodePath = "trayGarden/settings";
        protected Dictionary<string, string> Settings;  

        public virtual XmlDocument MainConfigurationNode
        {
            get
            {
                if (_mainConfigurationNode != null)
                    return _mainConfigurationNode;
                var mainSection = ConfigurationManager.GetSection("trayGarden") as SectionHandler;
                if (mainSection != null)
                    _mainConfigurationNode = mainSection.XmlRepresentation;
                else
                    _mainConfigurationNode = GetEmbeddedConfiguration();
                FilterNodes(_mainConfigurationNode);
                return _mainConfigurationNode;
            }
        }

        public virtual List<XmlNode> NodesWithDifferentNamespace { get; protected set; }


        protected virtual XmlDocument GetEmbeddedConfiguration()
        {
            var document = new XmlDocument();
            document.LoadXml(GlobalResourcesManager.GetStringByName("XmlConfiguration"));
            return document;
        }

        protected void FilterNodes(XmlDocument document)
        {
            NodesWithDifferentNamespace = new List<XmlNode>();
            var startNode = document.ChildNodes;
            foreach (XmlNode child in startNode)
            {
                FilterSingleNode(child);
            }
        }

        protected void FilterSingleNode(XmlNode parentNode)
        {
            var nodesFromOtherNamespace = new List<XmlNode>();
            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                if (childNode.NamespaceURI.NotNullNotEmpty())
                {
                    nodesFromOtherNamespace.Add(childNode);
                }
                FilterSingleNode(childNode);
            }
            foreach (XmlNode xmlNode in nodesFromOtherNamespace)
            {
                NodesWithDifferentNamespace.Add(xmlNode);
                parentNode.RemoveChild(xmlNode);
            }
        }

        private static ConfigurationBasedFactoryRaw _instance;
        private static object _lock = new object();
        public static ConfigurationBasedFactoryRaw ActualFactory
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock(_lock)
                {
                    if (_instance != null)
                        return _instance;
                    _instance = new ConfigurationBasedFactoryRaw();
                }
                return _instance;
            }
        }

        public virtual T GetObjectFromConfigurationNode<T>(XmlNode configurationNode) where T : class
        {

            if (configurationNode == null)
                return default(T);
            var typeString = configurationNode.Attributes["type"].Value;
            var objectType = ReflectionHelper.ResolveType(typeString);
            var resultObject = Activator.CreateInstance(objectType);
            var castedResultObject = resultObject as T;
            if (castedResultObject == null)
                return default(T);
            SetConfigurationNodeIfNeed(castedResultObject,configurationNode);
            return castedResultObject;
        }

        public virtual object GetObjectFromConfigurationNode(XmlNode configurationNode)
        {

            if (configurationNode == null)
                return null;
            var typeString = configurationNode.Attributes["type"].Value;
            var objectType = ReflectionHelper.ResolveType(typeString);
            var resultObject = Activator.CreateInstance(objectType);
            SetConfigurationNodeIfNeed(resultObject, configurationNode);
            return resultObject;
        }

        protected virtual void SetConfigurationNodeIfNeed<T>(T configurationNeederCandidate, XmlNode configurationNode)
        {
            var configurationNeeder = configurationNeederCandidate as INeedConfigurationNode;
            if (configurationNeeder != null)
            {
                configurationNeeder.SetConfigurationNode(configurationNode);
            }
        }

        public virtual T GetObjectFromConfigurationNode<T>(string objectPath) where T : class
        {
           // var appropriateNode = MainConfigurationNode.SelectSingleNode(objectPath);
            var appropriateNode = XmlHelper.SmartlySelectSingleNode(MainConfigurationNode, objectPath);
            return GetObjectFromConfigurationNode<T>(appropriateNode);
        }

        public virtual List<T> GetObjectsCollectionFromConfigurationNode<T>(string firstObjectPath) where T : class
        {

            if (string.IsNullOrEmpty(firstObjectPath))
                return default(List<T>);
            //var objectsNodes = MainConfigurationNode.SelectNodes(anyObjectPath);
            var objectsNodes = XmlHelper.SmartlySelectNodes(MainConfigurationNode, firstObjectPath);
            var resultingCollection = new List<T>();
            foreach (XmlNode objectsNode in objectsNodes)
            {
                var instantiatedObj = GetObjectFromConfigurationNode<T>(objectsNode);
                resultingCollection.Add(instantiatedObj);
            }
            return resultingCollection;

        }

        protected virtual void InitializeSettings()
        {
            Settings = new Dictionary<string, string>();
            var settingsParentNode = XmlHelper.SmartlySelectSingleNode(MainConfigurationNode, SettingsNodePath);
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

        public virtual string GetStringSetting(string settingName, string defaultValue)
        {
           if (Settings == null)
           {
               lock (_lock)
               {
                   if(Settings == null)
                       InitializeSettings();
               }
           }
            if (Settings.ContainsKey(settingName))
                return Settings[settingName];
            return defaultValue;
        }
    }
}
