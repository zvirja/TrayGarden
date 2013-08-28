using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.Clipboard
{
  public static class ClipboardManager
  {
    private static ClipboardProvider _provider;
    private static object _lock = new object();
    public static ClipboardProvider Provider
    {
      get
      {
        _provider = Factory.ActualFactory.GetClipboardProvider();
        return _provider;
      }
    }

    public static string GetValue()
    {
      return Provider.GetValue();
    }

    public static void SetValue(string newValue)
    {
      Provider.SetValue(newValue);
    }
  }
}
