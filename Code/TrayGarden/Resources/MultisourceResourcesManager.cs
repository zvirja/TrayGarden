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
  protected static object Lock = new object();

  public MultisourceResourcesManager()
  {
    this.StringsResourceCache = new Dictionary<string, string>();
    this.ObjectsResourceCache = new Dictionary<string, object>();
    this.Sources = new List<ISource>();
  }

  public List<ISource> Sources { get; set; }

  protected Dictionary<string, object> ObjectsResourceCache { get; set; }

  protected Dictionary<string, string> StringsResourceCache { get; set; }

  public virtual Bitmap GetBitmapResource(string resourceName, Bitmap defaultValue)
  {
    return this.GetObjectResource(resourceName, defaultValue);
  }

  public virtual Icon GetIconResource(string resourceName, Icon defaultValue)
  {
    return this.GetObjectResource(resourceName, defaultValue);
  }

  public virtual T GetObjectResource<T>(string resourceName, T defaultValue) where T : class
  {
    if (resourceName.IsNullOrEmpty())
    {
      return defaultValue;
    }
    object resolvedValue;
    lock (Lock)
    {
      if (this.ObjectsResourceCache.ContainsKey(resourceName))
      {
        var candidate = this.ObjectsResourceCache[resourceName] as T;
        return candidate ?? defaultValue;
      }
      resolvedValue = this.ResoveObjectFromSources(resourceName);
      this.ObjectsResourceCache.Add(resourceName, resolvedValue);
    }
    return (resolvedValue as T) ?? defaultValue;
  }

  public virtual Stream GetStream(string resourceName, Stream defaultValue)
  {
    return this.ResoveStreamFromSources(resourceName) ?? defaultValue;
  }

  public virtual string GetStringResource(string resourceName, string defaultValue)
  {
    if (resourceName.IsNullOrEmpty())
    {
      return defaultValue;
    }
    if (this.StringsResourceCache.ContainsKey(resourceName))
    {
      return this.StringsResourceCache[resourceName];
    }
    string resolvedValue;
    lock (Lock)
    {
      if (this.StringsResourceCache.ContainsKey(resourceName))
      {
        return this.StringsResourceCache[resourceName];
      }
      resolvedValue = this.ResoveStringFromSources(resourceName) ?? defaultValue;
      this.StringsResourceCache.Add(resourceName, resolvedValue);
    }
    return resolvedValue;
  }

  protected virtual T ResolveFromResources<T>(Func<ResourceManager, T> resolver, T defaultValue) where T : class
  {
    var sourcesToRemove = new List<ISource>();
    T resolvedValue = null;
    foreach (ISource source in this.Sources)
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
        Log.Error("Can't read resource!", ex, this);
      }
    }

    if (sourcesToRemove.Count > 0)
    {
      foreach (var source in sourcesToRemove)
      {
        this.Sources.Remove(source);
        Log.Info("Resource source was removed: {0}".FormatWith(source.Source.BaseName), this);
      }
    }

    return resolvedValue ?? defaultValue;
  }

  protected virtual object ResoveObjectFromSources(string resourceName)
  {
    return this.ResolveFromResources((rm) => rm.GetObject(resourceName), null);
  }

  protected virtual Stream ResoveStreamFromSources(string resourceName)
  {
    return this.ResolveFromResources((rm) => rm.GetStream(resourceName), null);
  }

  protected virtual string ResoveStringFromSources(string resourceName)
  {
    return this.ResolveFromResources((rm) => rm.GetString(resourceName), null);
  }
}