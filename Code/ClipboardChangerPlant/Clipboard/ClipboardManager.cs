using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.Clipboard;

public static class ClipboardManager
{
  private static object _lock = new object();

  private static ClipboardProvider _provider;

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

  public static void SetValue(string newValue, bool silent = false)
  {
    Provider.SetValue(newValue, silent);
  }
}