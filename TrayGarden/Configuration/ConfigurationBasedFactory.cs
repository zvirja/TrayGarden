using System;
using System.Collections.Generic;
using TrayGarden.Pipelines;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Resources;

namespace TrayGarden.Configuration
{
    public class ConfigurationBasedFactory
    {
        #region Core functionality

        private static readonly Lazy<ConfigurationBasedFactory> _instance = new Lazy<ConfigurationBasedFactory>(() => new ConfigurationBasedFactory());
        private static readonly object _lock = new object();

        public static ConfigurationBasedFactory ActualFactory
        {
            get { return _instance.Value; }
        }

        public ConfigurationBasedFactoryRaw RawFactory
        {
            get { return ConfigurationBasedFactoryRaw.ActualFactory; }
        }

        public virtual string GetStringSetting(string settingName, string defaultValue)
        {
            return RawFactory.GetStringSetting(settingName, defaultValue);
        }

        public virtual bool GetBoolSetting(string settingName, bool defaultValue = false)
        {
            var setting = GetStringSetting(settingName, string.Empty);
            if (string.IsNullOrEmpty(setting))
                return defaultValue;
            bool result;
            if (bool.TryParse(setting, out result))
                return result;
            return defaultValue;
        }

        protected Dictionary<string, object> _cache = new Dictionary<string, object>();

        protected virtual T GetFromFactoryCacheBased<T>(string cacheKey, Func<T> resolver) where T : class
        {
            if (_cache.ContainsKey(cacheKey))
                return _cache[cacheKey] as T;
            lock (_lock)
            {
                if (_cache.ContainsKey(cacheKey))
                    return _cache[cacheKey] as T;
                var resolved = resolver();
                _cache.Add(cacheKey, resolved);
                return resolved;
            }
        }

        protected virtual T GetSingleObjectFromRawFactory<T>(string xPath) where T: class
        {
            T instance = GetFromFactoryCacheBased(xPath, () => RawFactory.GetObjectFromConfigurationNode<T>(xPath));
            return instance;
        }

        protected virtual List<T> GetSingleObjectsCollectionFromRawFactory<T>(string xPath) where T : class
        {
            List<T> instance = GetFromFactoryCacheBased(xPath, () => RawFactory.GetObjectsCollectionFromConfigurationNode<T>(xPath));
            return instance;
        }



        #endregion

        #region Application specific part
   
        public virtual MultisourceResourcesManager GetMultisourceIconsManager()
        {
            const string xPath = "resourceManager";
            var instance = GetSingleObjectFromRawFactory<MultisourceResourcesManager>(xPath);
            return instance;
        }

        public virtual PipelineManager GetPipelineManager()
        {
            const string xPath = "pipelineManager";
            var instance = GetSingleObjectFromRawFactory<PipelineManager>(xPath);
            return instance;
        }

        public virtual TypesHatcher.HatcherManager GetHatcherManager()
        {
            const string xPath = "typeHatcherManager";
            var instance = GetSingleObjectFromRawFactory<TypesHatcher.HatcherManager>(xPath);
            return instance;
        }


        #endregion
    }
}
