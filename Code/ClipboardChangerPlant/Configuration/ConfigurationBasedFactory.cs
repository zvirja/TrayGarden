#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Engine;
using ClipboardChangerPlant.NotificationIcon;
using ClipboardChangerPlant.RequestHandling;
using ClipboardChangerPlant.Shortening;

#endregion

namespace ClipboardChangerPlant.Configuration
{
  public class ConfigurationBasedFactory
  {
    #region Static Fields

    private static ConfigurationBasedFactory _instance;

    private static object _lock = new object();

    #endregion

    #region Fields

    private Dictionary<string, object> _cache = new Dictionary<string, object>();

    #endregion

    #region Public Properties

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

    #endregion

    #region Public Methods and Operators

    public AppEngine GetApplicationEngine()
    {
      const string key = "applicationEngine";
      var engine = this.GetFromFactoryCacheBased(key, () => this.RawFactory.GetObjectFromConfigurationNode<AppEngine>("ApplicationEngine"));
      return engine;
    }

    public ClipboardProvider GetClipboardProvider()
    {
      const string key = "clipboardProvider";
      var provider = this.GetFromFactoryCacheBased(
        key,
        () => this.RawFactory.GetObjectFromConfigurationNode<ClipboardProvider>("ClipboardProvider"));
      return provider;
    }

    public NotifyIconManager GetNotifyIconManager()
    {
      const string key = "notifyIconManager";
      var manager = this.GetFromFactoryCacheBased(
        key,
        () => this.RawFactory.GetObjectFromConfigurationNode<NotifyIconManager>("NotifyIconManager"));
      return manager;
    }

    public List<RequestHandler> GetRequestHandlers()
    {
      const string key = "requestHandlers";
      var handlers = this.GetFromFactoryCacheBased(
        key,
        () => this.RawFactory.GetObjectsCollectionFromConfigurationNode<RequestHandler>("RequestHandlers/RequestHandler"));
      return handlers;
    }

    public ProcessManager GetRequestProcessManager()
    {
      const string key = "requestProcessManager";
      var manager = this.GetFromFactoryCacheBased(
        key,
        () => this.RawFactory.GetObjectFromConfigurationNode<ProcessManager>("ProcessManager"));
      return manager;
    }

    public List<ShortenerProvider> GetShortenerProviders()
    {
      const string key = "shortenerProviders";
      var providers = this.GetFromFactoryCacheBased(
        key,
        () => this.RawFactory.GetObjectsCollectionFromConfigurationNode<ShortenerProvider>("ShortenerProviders/ShortenerProvider"));
      return providers;
    }

    #endregion

    #region Methods

    private T GetFromFactoryCacheBased<T>(string cacheKey, Func<T> resolver) where T : class
    {
      if (this._cache.ContainsKey(cacheKey))
      {
        return this._cache[cacheKey] as T;
      }
      lock (_lock)
      {
        if (this._cache.ContainsKey(cacheKey))
        {
          return this._cache[cacheKey] as T;
        }
        var resolved = resolver();
        this._cache.Add(cacheKey, resolved);
        return resolved;
      }
    }

    #endregion
  }
}