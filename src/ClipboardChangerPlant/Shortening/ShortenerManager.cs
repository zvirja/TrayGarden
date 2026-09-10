using System.Collections.Generic;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.Shortening;

public class ShortenerManager
{
  private static object _lock = new object();

  private static List<ShortenerProvider> _providers;

  public static List<ShortenerProvider> Providers
  {
    get
    {
      if (_providers != null)
      {
        return _providers;
      }
      lock (_lock)
      {
        if (_providers != null)
        {
          return _providers;
        }
        _providers = Factory.ActualFactory.GetShortenerProviders();
      }
      return _providers;
    }
  }

  public static bool TryShorterUrl(string inputUrl, out string outputUrl)
  {
    foreach (var shortenerProvider in Providers)
    {
      if (shortenerProvider.TryShortUrl(inputUrl, out outputUrl))
      {
        return true;
      }
    }
    outputUrl = inputUrl;
    return false;
  }
}