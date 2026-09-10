using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using TrayGarden.Diagnostics;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace MicrosoftTodoTrayPlant;

// HWND_MESSAGE - a window parented here is message-only: no paint, no z-order, not enumerated.

/// <summary>
/// Registers a system-wide hotkey via a message-only window and invokes a callback when it fires.
/// Must be constructed on a thread with a running message loop (the WPF dispatcher thread).
/// </summary>
internal sealed class GlobalHotKey : IDisposable
{
  private const int HotKeyId = 0x5A01;

  private readonly HotKeyWindow _window;

  public GlobalHotKey(HOT_KEY_MODIFIERS modifiers, VIRTUAL_KEY key, string displayName, Action onPressed)
  {
    _window = new HotKeyWindow(modifiers | HOT_KEY_MODIFIERS.MOD_NOREPEAT, key, displayName, onPressed);
  }

  public void Dispose()
  {
    _window?.Dispose();
  }

  private sealed class HotKeyWindow : Form
  {
    private const int HwndMessage = -3;

    private readonly Action _onPressed;
    private readonly bool _registered;

    public HotKeyWindow(HOT_KEY_MODIFIERS modifiers, VIRTUAL_KEY key, string displayName, Action onPressed)
    {
      _onPressed = onPressed;

      _ = Handle; // force handle creation without showing the window

      _registered = PInvoke.RegisterHotKey((HWND)Handle, HotKeyId, modifiers, (uint)key);
      if (_registered)
      {
        Log.For(this).Information("Registered global hotkey '{HotKey}'.", displayName);
      }
      else
      {
        Log.For(this).Warning("RegisterHotKey failed for '{HotKey}' ({Error}).", displayName, Marshal.GetLastWin32Error());
      }
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams cp = base.CreateParams;
        cp.Parent = new IntPtr(HwndMessage);
        return cp;
      }
    }

    protected override void SetVisibleCore(bool value)
    {
      base.SetVisibleCore(false);
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == (int)PInvoke.WM_HOTKEY && m.WParam.ToInt32() == HotKeyId)
      {
        try
        {
          _onPressed();
        }
        catch (Exception ex)
        {
          Log.For(this).Error(ex, "Global hotkey handler threw.");
        }

        return;
      }

      base.WndProc(ref m);
    }

    protected override void Dispose(bool disposing)
    {
      if (_registered && !IsDisposed)
      {
        PInvoke.UnregisterHotKey((HWND)Handle, HotKeyId);
      }

      base.Dispose(disposing);
    }
  }
}
