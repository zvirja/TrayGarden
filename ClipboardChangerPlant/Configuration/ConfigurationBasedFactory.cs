using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Engine;
using ClipboardChangerPlant.RequestHandling;
using ClipboardChangerPlant.Shortening;
using ClipboardChangerV2.Configuration;
using ClipboardChangerV2.NotificationIcon;

namespace ClipboardChangerPlant.Configuration
{
  public class ConfigurationBasedFactory
  {
    private static ConfigurationBasedFactory _instance;
    private static object _lock = new object();

    public static ConfigurationBasedFactory ActualFactory
    {
      get
      {
        if (_instance != null)
          return _instance;
        lock (_lock)
        {
          if (_instance != null)
            return _instance;
          _instance = new ConfigurationBasedFactory();
        }
        return _instance;
      }
    }

    public ConfigurationBasedFactoryRaw RawFactory
    {
      get { return ConfigurationBasedFactoryRaw.ActualFactory; }
    }

    private Dictionary<string, object> _cache = new Dictionary<string, object>();

    private T GetFromFactoryCacheBased<T>(string cacheKey, Func<T> resolver) where T : class
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

    public List<RequestHandler> GetRequestHandlers()
    {
      const string key = "requestHandlers";
      var handlers = GetFromFactoryCacheBased(key,
                                              () =>
                                              RawFactory.GetObjectsCollectionFromConfigurationNode<RequestHandler>
                                                  (
                                                      "RequestHandlers/RequestHandler"));
      return handlers;
    }

    public List<ShortenerProvider> GetShortenerProviders()
    {
      const string key = "shortenerProviders";
      var providers = GetFromFactoryCacheBased(key,
                                               () =>
                                               RawFactory.GetObjectsCollectionFromConfigurationNode
                                                   <ShortenerProvider>("ShortenerProviders/ShortenerProvider"));
      return providers;
    }

    public ClipboardProvider GetClipboardProvider()
    {
      const string key = "clipboardProvider";
      var provider = GetFromFactoryCacheBased(key,
                                              () =>
                                              RawFactory.GetObjectFromConfigurationNode<ClipboardProvider>(
                                                  "ClipboardProvider"));
      return provider;
    }

    public NotifyIconManager GetNotifyIconManager()
    {
      const string key = "notifyIconManager";
      var manager = GetFromFactoryCacheBased(key,
                                             () =>
                                             RawFactory.GetObjectFromConfigurationNode<NotifyIconManager>(
                                                 "NotifyIconManager"));
      return manager;
    }

    public ProcessManager GetRequestProcessManager()
    {
      const string key = "requestProcessManager";
      var manager = GetFromFactoryCacheBased(key,
                                             () =>
                                             RawFactory.GetObjectFromConfigurationNode<ProcessManager>(
                                                 "ProcessManager"));
      return manager;
    }

    public AppEngine GetApplicationEngine()
    {
      const string key = "applicationEngine";
      var engine = GetFromFactoryCacheBased(key,
                                            () =>
                                            RawFactory.GetObjectFromConfigurationNode<AppEngine>(
                                                "ApplicationEngine"));
      return engine;
    }
  }
}
