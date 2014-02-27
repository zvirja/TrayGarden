#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Configuration;

#endregion

namespace ClipboardChangerPlant.Clipboard
{
  public static class ClipboardManager
  {
    #region Static Fields

    private static object _lock = new object();

    private static ClipboardProvider _provider;

    #endregion

    #region Public Properties

    public static ClipboardProvider Provider
    {
      get
      {
        _provider = Factory.ActualFactory.GetClipboardProvider();
        return _provider;
      }
    }

    #endregion

    #region Public Methods and Operators

    public static string GetValue()
    {
      return Provider.GetValue();
    }

    public static void SetValue(string newValue, bool silent = false)
    {
      Provider.SetValue(newValue, silent);
    }

    #endregion
  }
}