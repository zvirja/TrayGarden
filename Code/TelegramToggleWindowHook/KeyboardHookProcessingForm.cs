using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TrayGarden.Helpers;

namespace TelegramToggleWindowHook;

internal class KeyboardHookProcessingForm : Form
{
  private const int GWL_STYLE = -16;

  private const uint MOD_CONTROL = 0x0002;


  private const uint WM_HOTKEY = 0x0312;

  private const int WS_MINIMIZE = 0x20000000;
  private IntPtr _lastForegroundWindow;

  public KeyboardHookProcessingForm()
  {
    NativeHelper.SetParent(this.Handle, NativeHelper.HWND_MESSAGE);
    RegisterHotKey(this.Handle, 1, MOD_CONTROL, (uint)Keys.Oem3);
  }

  protected override void WndProc(ref Message m)
  {
    base.WndProc(ref m);

    if (m.Msg == WM_HOTKEY)
    {
      this.ToggleTelegramWindow();
    }
  }

  // For Windows Mobile, replace user32.dll with coredll.dll
  [DllImport("user32.dll", SetLastError = true)]
  private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

  [DllImport("user32.dll")]
  private static extern IntPtr GetForegroundWindow();

  [DllImport("user32.dll", SetLastError = true)]
  private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


  [DllImport("user32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);


  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool SetForegroundWindow(IntPtr hWnd);

  [DllImport("user32.dll")]
  private static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

  private void ToggleTelegramWindow()
  {
    var telegramProcess = Process.GetProcessesByName("telegram").FirstOrDefault();

    if (telegramProcess == null)
    {
      return;
    }

    var telegramWindow = telegramProcess.MainWindowHandle;
    if (telegramWindow == IntPtr.Zero)
    {
      return;
    }


    var isMinimizedNow = (GetWindowLong(telegramWindow, GWL_STYLE) & WS_MINIMIZE) != 0;
    var foregroundWindow = GetForegroundWindow();

    if (foregroundWindow == telegramWindow)
    {
      //If we are hiding this window - try to switch to the previous one.
      if (isMinimizedNow)
      {
        ShowWindow(telegramWindow, WindowShowStyle.ShowNormal);
      }
      else
      {
        ShowWindow(telegramWindow, WindowShowStyle.ShowMinimized);
        if (this._lastForegroundWindow != IntPtr.Zero)
        {
          SetForegroundWindow(this._lastForegroundWindow);
          this._lastForegroundWindow = IntPtr.Zero;
        }
      }
    }
    else
    {
      //Make the telegram window active. Store the current active window.
      this._lastForegroundWindow = GetForegroundWindow();

      ShowWindow(telegramWindow, WindowShowStyle.ShowNormal);
      SetForegroundWindow(telegramWindow);
    }
  }

  /// <summary>
  ///   Enumeration of the different ways of showing a window using
  ///   ShowWindow
  /// </summary>
  private enum WindowShowStyle : uint
  {
    /// <summary>Hides the window and activates another window.</summary>
    /// <remarks>See SW_HIDE</remarks>
    Hide = 0,

    /// <summary>
    ///   Activates and displays a window. If the window is minimized
    ///   or maximized, the system restores it to its original size and
    ///   position. An application should specify this flag when displaying
    ///   the window for the first time.
    /// </summary>
    /// <remarks>See SW_SHOWNORMAL</remarks>
    ShowNormal = 1,

    /// <summary>Activates the window and displays it as a minimized window.</summary>
    /// <remarks>See SW_SHOWMINIMIZED</remarks>
    ShowMinimized = 2,

    /// <summary>Activates the window and displays it as a maximized window.</summary>
    /// <remarks>See SW_SHOWMAXIMIZED</remarks>
    ShowMaximized = 3,

    /// <summary>Maximizes the specified window.</summary>
    /// <remarks>See SW_MAXIMIZE</remarks>
    Maximize = 3,

    /// <summary>
    ///   Displays a window in its most recent size and position.
    ///   This value is similar to "ShowNormal", except the window is not
    ///   actived.
    /// </summary>
    /// <remarks>See SW_SHOWNOACTIVATE</remarks>
    ShowNormalNoActivate = 4,

    /// <summary>
    ///   Activates the window and displays it in its current size
    ///   and position.
    /// </summary>
    /// <remarks>See SW_SHOW</remarks>
    Show = 5,

    /// <summary>
    ///   Minimizes the specified window and activates the next
    ///   top-level window in the Z order.
    /// </summary>
    /// <remarks>See SW_MINIMIZE</remarks>
    Minimize = 6,

    /// <summary>
    ///   Displays the window as a minimized window. This value is
    ///   similar to "ShowMinimized", except the window is not activated.
    /// </summary>
    /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
    ShowMinNoActivate = 7,

    /// <summary>
    ///   Displays the window in its current size and position. This
    ///   value is similar to "Show", except the window is not activated.
    /// </summary>
    /// <remarks>See SW_SHOWNA</remarks>
    ShowNoActivate = 8,

    /// <summary>
    ///   Activates and displays the window. If the window is
    ///   minimized or maximized, the system restores it to its original size
    ///   and position. An application should specify this flag when restoring
    ///   a minimized window.
    /// </summary>
    /// <remarks>See SW_RESTORE</remarks>
    Restore = 9,

    /// <summary>
    ///   Sets the show state based on the SW_ value specified in the
    ///   STARTUPINFO structure passed to the CreateProcess function by the
    ///   program that started the application.
    /// </summary>
    /// <remarks>See SW_SHOWDEFAULT</remarks>
    ShowDefault = 10,

    /// <summary>
    ///   Windows 2000/XP: Minimizes a window, even if the thread
    ///   that owns the window is hung. This flag should only be used when
    ///   minimizing windows from a different thread.
    /// </summary>
    /// <remarks>See SW_FORCEMINIMIZE</remarks>
    ForceMinimized = 11
  }
}