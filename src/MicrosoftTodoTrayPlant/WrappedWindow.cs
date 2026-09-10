using TrayGarden.Diagnostics;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace MicrosoftTodoTrayPlant;

/// <summary>
/// Drives a foreign top-level window: disables its close button so a click on X is inert,
/// and hides / shows it on request. Restores the close button when released.
/// </summary>
internal sealed class WrappedWindow
{
  private HWND _hWnd;
  private bool _closeDisabled;

  public HWND Handle => _hWnd;

  public bool Exists => !_hWnd.IsNull && PInvoke.IsWindow(_hWnd);

  public bool IsHidden => Exists && !PInvoke.IsWindowVisible(_hWnd);

  public void Wrap(HWND hWnd)
  {
    _hWnd = hWnd;
    DisableCloseButton();
  }

  public void Hide()
  {
    if (Exists)
    {
      PInvoke.ShowWindow(_hWnd, SHOW_WINDOW_CMD.SW_HIDE);
    }
  }

  public void Show()
  {
    if (!Exists)
    {
      return;
    }

    PInvoke.ShowWindow(_hWnd, SHOW_WINDOW_CMD.SW_SHOW);

    var style = (WINDOW_STYLE)(uint)PInvoke.GetWindowLong(_hWnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE);
    if ((style & WINDOW_STYLE.WS_MINIMIZE) != 0)
    {
      PInvoke.ShowWindow(_hWnd, SHOW_WINDOW_CMD.SW_RESTORE);
    }

    PInvoke.SetForegroundWindow(_hWnd);
  }

  public void Release()
  {
    RestoreCloseButton();
    if (Exists)
    {
      PInvoke.ShowWindow(_hWnd, SHOW_WINDOW_CMD.SW_SHOW);
    }

    _hWnd = default;
  }

  private void DisableCloseButton()
  {
    if (!Exists)
    {
      return;
    }

    HMENU menu = PInvoke.GetSystemMenu(_hWnd, false);
    if (menu.IsNull)
    {
      Log.For(this).Warning("GetSystemMenu returned null; close button cannot be disabled.");
      return;
    }

    PInvoke.EnableMenuItem(menu, PInvoke.SC_CLOSE, MENU_ITEM_FLAGS.MF_BYCOMMAND | MENU_ITEM_FLAGS.MF_GRAYED);
    PInvoke.DrawMenuBar(_hWnd);
    _closeDisabled = true;
  }

  private void RestoreCloseButton()
  {
    if (!_closeDisabled || !Exists)
    {
      _closeDisabled = false;
      return;
    }

    HMENU menu = PInvoke.GetSystemMenu(_hWnd, false);
    if (!menu.IsNull)
    {
      PInvoke.EnableMenuItem(menu, PInvoke.SC_CLOSE, MENU_ITEM_FLAGS.MF_BYCOMMAND | MENU_ITEM_FLAGS.MF_ENABLED);
      PInvoke.DrawMenuBar(_hWnd);
    }

    _closeDisabled = false;
  }
}
