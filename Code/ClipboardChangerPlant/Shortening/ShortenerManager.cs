#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Configuration;

#endregion

namespace ClipboardChangerPlant.Shortening
{
  public class ShortenerManager
  {
    #region Static Fields

    private static object _lock = new object();

    private static List<ShortenerProvider> _providers;

    #endregion

    #region Public Properties

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

    #endregion

    #region Public Methods and Operators

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

    #endregion
  }
}