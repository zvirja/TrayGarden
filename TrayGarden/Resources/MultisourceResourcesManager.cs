using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.Resources
{
    public class MultisourceResourcesManager : IRequireInitialization, IResourcesManager
    {
        protected static object Lock = new object();
        protected List<ResourceManager> ResolvedSources { get; set; }
        protected Dictionary<string, string> StringsResourceCache { get; set; }
        protected Dictionary<string, Icon> IconsResourceCache { get; set; }

        public List<ISource> Sources { get; set; }


        public MultisourceResourcesManager()
        {
            StringsResourceCache = new Dictionary<string, string>();
            IconsResourceCache = new Dictionary<string, Icon>();
            Sources = new List<ISource>();
            ResolvedSources = new List<ResourceManager>();
        }

        public virtual void Initialize()
        {
            foreach (ISource source in Sources)
            {
                var sourceManager = source.Source;
                if (sourceManager != null)
                    ResolvedSources.Add(sourceManager);
            }
        }

        protected virtual T ResolveFromResources<T>(Func<ResourceManager, T> resolver, T defaultValue) where T : class
        {
            var sourcesToRemove = new List<ResourceManager>();
            T resolvedValue = null;
            foreach (ResourceManager resourceSource in ResolvedSources)
            {
                try
                {
                    resolvedValue = resolver(resourceSource);
                    if (resolvedValue != null)
                        break;
                }
                catch (MissingManifestResourceException)
                {
                    sourcesToRemove.Add(resourceSource);
                }
            }

            if (sourcesToRemove.Count > 0)
            {
                foreach (var source in sourcesToRemove)
                {
                    ResolvedSources.Remove(source);
                }
            }

            return resolvedValue ?? defaultValue;
        }


        public virtual string GetStringResource(string resourceName, string defaultValue)
        {
            if (resourceName.IsNullOrEmpty())
                return defaultValue;
            if (StringsResourceCache.ContainsKey(resourceName))
                return StringsResourceCache[resourceName];
            string resolvedValue;
            lock (Lock)
            {
                if (StringsResourceCache.ContainsKey(resourceName))
                    return StringsResourceCache[resourceName];
                resolvedValue = ResoveStringFromSources(resourceName) ?? defaultValue;
                StringsResourceCache.Add(resourceName, resolvedValue);
            }
            return resolvedValue;
        }

        protected virtual string ResoveStringFromSources(string resourceName)
        {
            return ResolveFromResources((rm) => rm.GetString(resourceName), null);
        }

        public virtual Icon GetIconResource(string resourceName, Icon defaultValue)
        {
            if (resourceName.IsNullOrEmpty())
                return defaultValue;
            if (IconsResourceCache.ContainsKey(resourceName))
                return IconsResourceCache[resourceName];
            Icon resolvedValue;
            lock (Lock)
            {
                if (IconsResourceCache.ContainsKey(resourceName))
                    return IconsResourceCache[resourceName];
                resolvedValue = ResoveIconFromSources(resourceName) ?? defaultValue;
                IconsResourceCache.Add(resourceName, resolvedValue);
            }
            return resolvedValue;
        }

        protected virtual Icon ResoveIconFromSources(string resourceName)
        {
            return ResolveFromResources((rm) => rm.GetObject(resourceName), null) as Icon;
        }
    }
}