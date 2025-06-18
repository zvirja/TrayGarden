using System;
using System.Collections.Generic;
using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Engine;
using ClipboardChangerPlant.NotificationIcon;
using ClipboardChangerPlant.RequestHandling;
using ClipboardChangerPlant.Shortening;

namespace ClipboardChangerPlant.Configuration;

public class ConfigurationBasedFactory
{
  private static ConfigurationBasedFactory _instance;

  private static object _lock = new object();

  private Dictionary<string, object> _cache = new Dictionary<string, object>();

  public static ConfigurationBasedFactory ActualFactory
  {
    get
    {
      if (_instance != null)
      {
        return _instance;
      }
      lock (_lock)
      {
        if (_instance != null)
        {
          return _instance;
        }
        _instance = new ConfigurationBasedFactory();
      }
      return _instance;
    }
  }

  public ConfigurationBasedFactoryRaw RawFactory
  {
    get
    {
      return ConfigurationBasedFactoryRaw.ActualFactory;
    }
  }

  public AppEngine GetApplicationEngine()
  {
    const string key = "applicationEngine";
    var engine = GetFromFactoryCacheBased(key, () => RawFactory.GetObjectFromConfigurationNode<AppEngine>("ApplicationEngine"));
    return engine;
  }

  public ClipboardProvider GetClipboardProvider()
  {
    const string key = "clipboardProvider";
    var provider = GetFromFactoryCacheBased(
      key,
      () => RawFactory.GetObjectFromConfigurationNode<ClipboardProvider>("ClipboardProvider"));
    return provider;
  }

  public NotifyIconManager GetNotifyIconManager()
  {
    const string key = "notifyIconManager";
    var manager = GetFromFactoryCacheBased(
      key,
      () => RawFactory.GetObjectFromConfigurationNode<NotifyIconManager>("NotifyIconManager"));
    return manager;
  }

  public List<RequestHandler> GetRequestHandlers()
  {
    const string key = "requestHandlers";
    var handlers = GetFromFactoryCacheBased(
      key,
      () => RawFactory.GetObjectsCollectionFromConfigurationNode<RequestHandler>("RequestHandlers/RequestHandler"));
    return handlers;
  }

  public ProcessManager GetRequestProcessManager()
  {
    const string key = "requestProcessManager";
    var manager = GetFromFactoryCacheBased(
      key,
      () => RawFactory.GetObjectFromConfigurationNode<ProcessManager>("ProcessManager"));
    return manager;
  }

  public List<ShortenerProvider> GetShortenerProviders()
  {
    const string key = "shortenerProviders";
    var providers = GetFromFactoryCacheBased(
      key,
      () => RawFactory.GetObjectsCollectionFromConfigurationNode<ShortenerProvider>("ShortenerProviders/ShortenerProvider"));
    return providers;
  }

  private T GetFromFactoryCacheBased<T>(string cacheKey, Func<T> resolver) where T : class
  {
    if (_cache.ContainsKey(cacheKey))
    {
      return _cache[cacheKey] as T;
    }
    lock (_lock)
    {
      if (_cache.ContainsKey(cacheKey))
      {
        return _cache[cacheKey] as T;
      }
      var resolved = resolver();
      _cache.Add(cacheKey, resolved);
      return resolved;
    }
  }
}