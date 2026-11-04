using System;
using System.Windows;

namespace DevDashboardPlant;

/// <summary>
/// Owns the single web window. All members may be called from any thread; work is marshalled to the UI thread.
/// </summary>
internal sealed class DevDashboardController
{
  public static DevDashboardController Instance { get; } = new();

  private DevDashboardWindow _window;
  private bool _shutdown;

  private DevDashboardController()
  {
  }

  public void Toggle()
  {
    Dispatch(() =>
    {
      if (_shutdown)
      {
        return;
      }

      if (_window is { IsVisible: true, WindowState: not WindowState.Minimized } && _window.WasFocusedBeforeTrayClick())
      {
        _window.Hide();
        return;
      }

      EnsureWindow().ShowAndActivate();
    });
  }

  public void Reload(bool bypassCache)
  {
    Dispatch(() =>
    {
      if (!_shutdown)
      {
        EnsureWindow().ReloadPage(bypassCache);
      }
    });
  }

  public void Shutdown()
  {
    _shutdown = true;
    _window?.CloseForReal();
    _window = null;
  }

  private DevDashboardWindow EnsureWindow()
  {
    if (_window == null)
    {
      _window = new DevDashboardWindow();
    }

    return _window;
  }

  private static void Dispatch(Action action)
  {
    Application.Current?.Dispatcher.BeginInvoke(action);
  }
}
