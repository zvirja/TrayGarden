using System;

using TrayGarden.Diagnostics;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Accessibility;

namespace MicrosoftTodoTrayPlant;

/// <summary>
/// Out-of-process WinEvent hook that fires when the wrapped window starts to minimize,
/// so the controller can hide it instead of letting it land on the taskbar.
/// Must be created on a thread with a running message loop (the WPF dispatcher thread).
/// </summary>
internal sealed class MinimizeWatcher : IDisposable
{
  private const int OBJID_WINDOW = 0;

  private readonly HWND _target;
  private readonly Action _onMinimizeStart;
  private readonly WINEVENTPROC _proc;
  private UnhookWinEventSafeHandle _hook;

  public MinimizeWatcher(HWND target, uint processId, uint threadId, Action onMinimizeStart)
  {
    _target = target;
    _onMinimizeStart = onMinimizeStart;
    _proc = OnWinEvent;

    _hook = PInvoke.SetWinEventHook(
      PInvoke.EVENT_SYSTEM_MINIMIZESTART,
      PInvoke.EVENT_SYSTEM_MINIMIZESTART,
      null,
      _proc,
      processId,
      threadId,
      PInvoke.WINEVENT_OUTOFCONTEXT | PInvoke.WINEVENT_SKIPOWNPROCESS);

    if (_hook is null || _hook.IsInvalid)
    {
      Log.For(this).Warning("SetWinEventHook failed; minimize will not be redirected to the tray.");
    }
  }

  public void Dispose()
  {
    _hook?.Dispose();
    _hook = null;
  }

  private void OnWinEvent(
    HWINEVENTHOOK hWinEventHook,
    uint eventType,
    HWND hwnd,
    int idObject,
    int idChild,
    uint dwEventThread,
    uint dwmsEventTime)
  {
    if (idObject != OBJID_WINDOW || hwnd != _target)
    {
      return;
    }

    try
    {
      _onMinimizeStart();
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Minimize handler threw.");
    }
  }
}
