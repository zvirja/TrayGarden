using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    protected Dictionary<string, object> ObjectsResourceCache { get; set; }


    public List<ISource> Sources { get; set; }


    public MultisourceResourcesManager()
    {
      StringsResourceCache = new Dictionary<string, string>();
      ObjectsResourceCache = new Dictionary<string, object>();
      Sources = new List<ISource>();
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

    public virtual T GetObjectResource<T>(string resourceName, T defaultValue) where T : class
    {
      if (resourceName.IsNullOrEmpty())
        return defaultValue;
      object resolvedValue;
      lock (Lock)
      {
        if (ObjectsResourceCache.ContainsKey(resourceName))
        {
          var candidate = ObjectsResourceCache[resourceName] as T;
          return candidate ?? defaultValue;
        }
        resolvedValue = ResoveObjectFromSources(resourceName);
        ObjectsResourceCache.Add(resourceName, resolvedValue);
      }
      return (resolvedValue as T) ?? defaultValue;
    }

    public virtual Stream GetStream(string resourceName, Stream defaultValue)
    {
      return ResoveStreamFromSources(resourceName) ?? defaultValue;
    }

    public virtual Icon GetIconResource(string resourceName, Icon defaultValue)
    {
      return GetObjectResource(resourceName, defaultValue);
    }

    public virtual Bitmap GetBitmapResource(string resourceName, Bitmap defaultValue)
    {
      return GetObjectResource(resourceName, defaultValue);
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

    protected virtual string ResoveStringFromSources(string resourceName)
    {
      return ResolveFromResources((rm) => rm.GetString(resourceName), null);
    }

    protected virtual object ResoveObjectFromSources(string resourceName)
    {
      return ResolveFromResources((rm) => rm.GetObject(resourceName), null);
    }

     protected virtual Stream ResoveStreamFromSources(string resourceName)
     {
       return ResolveFromResources((rm) => rm.GetStream(resourceName), null);
     }
  }
}