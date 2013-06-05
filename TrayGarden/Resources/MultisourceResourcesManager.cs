using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.Resources
{
    [UsedImplicitly]
    public class MultisourceResourcesManager : IResourcesManager
    {
        protected static object Lock = new object();
        protected Dictionary<string, string> StringsResourceCache { get; set; }
        protected Dictionary<string, Icon> IconsResourceCache { get; set; }

        public List<ISource> Sources { get; set; }


        public MultisourceResourcesManager()
        {
            StringsResourceCache = new Dictionary<string, string>();
            IconsResourceCache = new Dictionary<string, Icon>();
            Sources = new List<ISource>();
        }


        protected virtual T ResolveFromResources<T>(Func<ResourceManager, T> resolver, T defaultValue) where T : class
        {
            var sourcesToRemove = new List<ISource>();
            T resolvedValue = null;
            foreach (ISource source in Sources)
            {
                var resourceSource = source.Source;
                if (resourceSource == null)
                {
                    sourcesToRemove.Add(source);
                    continue;
                }
                try
                {
                    resolvedValue = resolver(resourceSource);
                    if (resolvedValue != null)
                        break;
                }
                catch (MissingManifestResourceException)
                {
                    sourcesToRemove.Add(source);
                }
                catch (Exception ex)
                {
                    Log.Error("Can't read resource!", ex, this);
                }
            }

            if (sourcesToRemove.Count > 0)
            {
                foreach (var source in sourcesToRemove)
                {
                    Sources.Remove(source);
                    Log.Info("Resource source was removed: {0}".FormatWith(source.Source.BaseName), this);
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