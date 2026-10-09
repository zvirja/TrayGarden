using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UngoogledChromiumUpdaterPlant;

public static class ChromiumCloser
{
  private const string BrowserWindowClass = "Chrome_WidgetWin_1";
  private const uint WmCommand = 0x0111;

  /// <summary>
  /// Chromium's internal command id behind File > Exit (Ctrl+Shift+Q).
  /// </summary>
  private const int IdcExit = 34031;

  private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

  public static bool IsRunning(InstalledChromium installed)
  {
    return GetRunningProcessIds(installed).Count > 0;
  }

  /// <summary>
  /// Asks the running browser to exit the way the Exit menu item does, so every window is closed and the session is saved.
  /// Nothing is killed.
  /// </summary>
  /// <returns>False when the browser did not exit within the timeout, e.g. a page blocks closing with a prompt.</returns>
  public static async Task<bool> ExitGracefullyAsync(InstalledChromium installed, TimeSpan timeout)
  {
    HashSet<uint> pids = GetRunningProcessIds(installed);
    if (pids.Count == 0)
    {
      return true;
    }

    IntPtr window = FindBrowserWindows(pids).FirstOrDefault();
    if (window != IntPtr.Zero)
    {
      PostMessage(window, WmCommand, (IntPtr)IdcExit, IntPtr.Zero);
    }

    DateTime deadline = DateTime.UtcNow + timeout;
    while (DateTime.UtcNow < deadline)
    {
      if (GetRunningProcessIds(installed).Count == 0)
      {
        return true;
      }

      await Task.Delay(250);
    }

    return GetRunningProcessIds(installed).Count == 0;
  }

  private static HashSet<uint> GetRunningProcessIds(InstalledChromium installed)
  {
    var result = new HashSet<uint>();
    foreach (Process process in Process.GetProcessesByName("chrome"))
    {
      using (process)
      {
        try
        {
          string path = process.MainModule?.FileName;
          if (path != null && string.Equals(path, installed.ExePath, StringComparison.OrdinalIgnoreCase))
          {
            result.Add((uint)process.Id);
          }
        }
        catch (Exception)
        {
          // Not accessible or exited meanwhile: not our browser instance.
        }
      }
    }

    return result;
  }

  private static List<IntPtr> FindBrowserWindows(HashSet<uint> pids)
  {
    var windows = new List<IntPtr>();
    var className = new StringBuilder(256);
    EnumWindows(
      (handle, _) =>
      {
        GetWindowThreadProcessId(handle, out uint pid);
        if (pids.Contains(pid) && IsWindowVisible(handle))
        {
          className.Clear();
          GetClassName(handle, className, className.Capacity);
          if (className.ToString() == BrowserWindowClass)
          {
            windows.Add(handle);
          }
        }

        return true;
      },
      IntPtr.Zero);
    return windows;
  }

  [DllImport("user32.dll")]
  private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr lParam);

  [DllImport("user32.dll")]
  private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

  [DllImport("user32.dll", CharSet = CharSet.Unicode)]
  private static extern int GetClassName(IntPtr hWnd, StringBuilder className, int maxCount);

  [DllImport("user32.dll")]
  private static extern bool IsWindowVisible(IntPtr hWnd);

  [DllImport("user32.dll")]
  private static extern bool PostMessage(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam);
}
