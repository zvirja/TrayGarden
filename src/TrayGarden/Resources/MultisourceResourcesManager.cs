using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Resources;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.Resources;

[UsedImplicitly]
public class MultisourceResourcesManager : IResourcesManager
{
  private static object Lock = new object();

  public MultisourceResourcesManager()
  {
    StringsResourceCache = new Dictionary<string, string>();
    ObjectsResourceCache = new Dictionary<string, object>();
    Sources = new List<ISource>();
  }

  public List<ISource> Sources { get; set; }

  private Dictionary<string, object> ObjectsResourceCache { get; set; }

  private Dictionary<string, string> StringsResourceCache { get; set; }

  public Bitmap GetBitmapResource(string resourceName, Bitmap defaultValue)
  {
    return GetObjectResource(resourceName, defaultValue);
  }

  public Icon GetIconResource(string resourceName, Icon defaultValue)
  {
    return GetObjectResource(resourceName, defaultValue);
  }

  public T GetObjectResource<T>(string resourceName, T defaultValue) where T : class
  {
    if (resourceName.IsNullOrEmpty())
    {
      return defaultValue;
    }
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

  public Stream GetStream(string resourceName, Stream defaultValue)
  {
    return ResoveStreamFromSources(resourceName) ?? defaultValue;
  }

  public string GetStringResource(string resourceName, string defaultValue)
  {
    if (resourceName.IsNullOrEmpty())
    {
      return defaultValue;
    }
    if (StringsResourceCache.ContainsKey(resourceName))
    {
      return StringsResourceCache[resourceName];
    }
    string resolvedValue;
    lock (Lock)
    {
      if (StringsResourceCache.ContainsKey(resourceName))
      {
        return StringsResourceCache[resourceName];
      }
      resolvedValue = ResoveStringFromSources(resourceName) ?? defaultValue;
      StringsResourceCache.Add(resourceName, resolvedValue);
    }
    return resolvedValue;
  }

  private T ResolveFromResources<T>(Func<ResourceManager, T> resolver, T defaultValue) where T : class
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
        {
          break;
        }
      }
      catch (MissingManifestResourceException)
      {
        sourcesToRemove.Add(source);
      }
      catch (Exception ex)
      {
        Log.For(this).Error(ex, "Can't read resource!");
      }
    }

    if (sourcesToRemove.Count > 0)
    {
      foreach (var source in sourcesToRemove)
      {
        Sources.Remove(source);
        Log.For(this).Information("Resource source was removed: {SourceBaseName}", source.Source.BaseName);
      }
    }

    return resolvedValue ?? defaultValue;
  }

  private object ResoveObjectFromSources(string resourceName)
  {
    return ResolveFromResources((rm) => rm.GetObject(resourceName), null);
  }

  private Stream ResoveStreamFromSources(string resourceName)
  {
    return ResolveFromResources((rm) => rm.GetStream(resourceName), null);
  }

  private string ResoveStringFromSources(string resourceName)
  {
    return ResolveFromResources((rm) => rm.GetString(resourceName), null);
  }
}