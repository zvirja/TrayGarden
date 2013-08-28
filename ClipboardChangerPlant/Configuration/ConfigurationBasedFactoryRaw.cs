using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace ClipboardChangerPlant.Configuration
{
  public class ConfigurationBasedFactoryRaw
  {
    private static XmlDocument _mainConfigurationNode;
    public static XmlDocument MainConfigurationNode
    {
      get
      {
        if (_mainConfigurationNode != null)
          return _mainConfigurationNode;
        var mainSection = ConfigurationManager.GetSection("ClipboardChangerPlant") as SectionHandler;
        if (mainSection != null)
          _mainConfigurationNode = mainSection.XmlRepresentation;
        else
          _mainConfigurationNode = GetEmbeddedConfiguration();
        return _mainConfigurationNode;
      }
    }

    private static XmlDocument GetEmbeddedConfiguration()
    {
      var document = new XmlDocument();
      document.LoadXml(ResourcesOperator.GetStringByName("XmlConfiguration"));
      return document;
    }

    private static ConfigurationBasedFactoryRaw _instance;
    private static object _lock = new object();
    public static ConfigurationBasedFactoryRaw ActualFactory
    {
      get
      {
        if (_instance != null)
          return _instance;
        lock (_lock)
        {
          if (_instance != null)
            return _instance;
          _instance = new ConfigurationBasedFactoryRaw();
        }
        return _instance;
      }
    }

    public T GetObjectFromConfigurationNode<T>(XmlNode configurationNode) where T : class
    {

      if (configurationNode == null)
        return default(T);
      var typeString = configurationNode.Attributes["type"].Value;
      var objectType = Type.GetType(typeString, true);
      var resultObject = Activator.CreateInstance(objectType);
      var castedResultObject = resultObject as T;
      if (castedResultObject == null)
        return default(T);
      if (castedResultObject is INeedCongurationNode)
      {
        var configurationNeeder = (INeedCongurationNode)castedResultObject;
        configurationNeeder.SetConfigurationNode(configurationNode);
        configurationNeeder.Name = configurationNode.Attributes["name"].Value;
      }
      return castedResultObject;
    }

    public T GetObjectFromConfigurationNode<T>(string objectPath) where T : class
    {
      // var appropriateNode = MainConfigurationNode.SelectSingleNode(objectPath);
      var appropriateNode = XmlHelper.SmartSelectSingleNode(MainConfigurationNode, objectPath);
      return GetObjectFromConfigurationNode<T>(appropriateNode);
    }

    public List<T> GetObjectsCollectionFromConfigurationNode<T>(string firstObjectPath) where T : class
    {

      if (string.IsNullOrEmpty(firstObjectPath))
        return default(List<T>);
      //var objectsNodes = MainConfigurationNode.SelectNodes(anyObjectPath);
      var objectsNodes = XmlHelper.SmartSelectNodes(MainConfigurationNode, firstObjectPath);
      var resultingCollection = new List<T>();
      foreach (XmlNode objectsNode in objectsNodes)
      {
        var instantiatedObj = GetObjectFromConfigurationNode<T>(objectsNode);
        resultingCollection.Add(instantiatedObj);
      }
      return resultingCollection;

    }

    public string GetStringSetting(string settingName, string defaultValue)
    {
      var setting = ConfigurationManager.AppSettings[settingName];
      if (string.IsNullOrEmpty(setting))
        return defaultValue;
      return setting;
    }

    public bool GetBoolSetting(string settingName, bool defaultValue = false)
    {
      var setting = ConfigurationManager.AppSettings[settingName];
      if (string.IsNullOrEmpty(setting))
        return defaultValue;
      bool result;
      if (bool.TryParse(setting, out result))
        return result;
      return defaultValue;
    }
  }
}
