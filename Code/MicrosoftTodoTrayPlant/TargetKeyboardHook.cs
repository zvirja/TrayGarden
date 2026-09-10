using System;
using System.Runtime.InteropServices;

using TrayGarden.Diagnostics;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Windows.Win32.UI.WindowsAndMessaging;

namespace MicrosoftTodoTrayPlant;

/// <summary>
/// Low-level keyboard hook. While the wrapped window is the foreground (root) window it redirects
/// Alt+F4 to "hide to tray", and - when enabled - does the same for Esc. A low-level hook runs
/// before the app, so it cannot tell whether the app would itself act on Esc; the setting exists
/// so the user can turn the unconditional behaviour off.
/// </summary>
internal sealed class TargetKeyboardHook : IDisposable
{
  private readonly Func<HWND> _targetProvider;
  private readonly Func<bool> _hideOnEscapeEnabled;
  private readonly Action _hideRequested;
  private readonly HOOKPROC _proc;
  private UnhookWindowsHookExSafeHandle _hookHandle;

  public TargetKeyboardHook(Func<HWND> targetProvider, Func<bool> hideOnEscapeEnabled, Action hideRequested)
  {
    _targetProvider = targetProvider;
    _hideOnEscapeEnabled = hideOnEscapeEnabled;
    _hideRequested = hideRequested;
    _proc = HookCallback;
  }

  public void Install()
  {
    if (_hookHandle is { IsInvalid: false })
    {
      return;
    }

    _hookHandle = PInvoke.SetWindowsHookEx(
      WINDOWS_HOOK_ID.WH_KEYBOARD_LL, _proc, PInvoke.GetModuleHandle((string)null), 0);

    if (_hookHandle is null || _hookHandle.IsInvalid)
    {
      Log.For(this).Warning("Failed to install low-level keyboard hook; Alt+F4 / Esc will not be redirected.");
    }
  }

  public void Dispose()
  {
    _hookHandle?.Dispose();
    _hookHandle = null;
  }

  private LRESULT HookCallback(int nCode, WPARAM wParam, LPARAM lParam)
  {
    if (nCode >= 0 && ShouldRedirect((uint)wParam.Value, lParam))
    {
      try
      {
        _hideRequested();
      }
      catch (Exception ex)
      {
        Log.For(this).Error(ex, "Keyboard hide handler threw.");
      }

      return (LRESULT)1;
    }

    return PInvoke.CallNextHookEx(_hookHandle, nCode, wParam, lParam);
  }

  private bool ShouldRedirect(uint message, LPARAM lParam)
  {
    var data = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

    bool altF4 = message == PInvoke.WM_SYSKEYDOWN
                 && (VIRTUAL_KEY)data.vkCode == VIRTUAL_KEY.VK_F4
                 && (data.flags & KBDLLHOOKSTRUCT_FLAGS.LLKHF_ALTDOWN) != 0;

    bool escape = message == PInvoke.WM_KEYDOWN
                  && (VIRTUAL_KEY)data.vkCode == VIRTUAL_KEY.VK_ESCAPE
                  && _hideOnEscapeEnabled();

    if (!altF4 && !escape)
    {
      return false;
    }

    return IsTargetInForeground();
  }

  private bool IsTargetInForeground()
  {
    HWND target = _targetProvider();
    return !target.IsNull && PInvoke.GetForegroundWindow() == target;
  }
}
