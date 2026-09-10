using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using TrayGarden.Diagnostics;

using Windows.Win32;
using Windows.Win32.Foundation;

namespace MicrosoftTodoTrayPlant;

internal sealed class WrappedTarget
{
  public WrappedTarget(HWND frameWindow, Process process)
  {
    FrameWindow = frameWindow;
    Process = process;
  }

  public HWND FrameWindow { get; }

  public Process Process { get; }
}

/// <summary>
/// Starts the target app and resolves the top-level window to wrap plus the process that backs it.
/// </summary>
internal static class TodoLauncher
{
  public static void StartProcess(PlantConfiguration config)
  {
    string command = config.LaunchCommand.Value;
    Log.For(typeof(TodoLauncher)).Information("Launching target app: {Command}", command);
    Process.Start(new ProcessStartInfo(command) { UseShellExecute = true });
  }

  public static Process FindProcess(PlantConfiguration config)
  {
    return Process.GetProcessesByName(config.ProcessName.Value)
      .Where(p =>
      {
        try
        {
          return !p.HasExited;
        }
        catch
        {
          return false;
        }
      })
      .OrderBy(p =>
      {
        try
        {
          return p.StartTime;
        }
        catch
        {
          return DateTime.MaxValue;
        }
      })
      .FirstOrDefault();
  }

  public static WrappedTarget WaitForTarget(PlantConfiguration config, TimeSpan timeout)
  {
    string wantedClass = config.FrameWindowClass.Value ?? string.Empty;
    string wantedTitle = config.FrameWindowTitle.Value ?? string.Empty;
    DateTime deadline = DateTime.UtcNow + timeout;

    while (DateTime.UtcNow < deadline)
    {
      Process process = FindProcess(config);
      HWND frame = FindFrameWindow(wantedClass, wantedTitle);

      if (!frame.IsNull && process != null)
      {
        Log.For(typeof(TodoLauncher)).Information("Resolved wrap target. Pid={Pid}", process.Id);
        return new WrappedTarget(frame, process);
      }

      Thread.Sleep(250);
    }

    Log.For(typeof(TodoLauncher)).Warning(
      "Timed out waiting for wrap target (class='{Class}', title='{Title}')", wantedClass, wantedTitle);
    return null;
  }

  private static HWND FindFrameWindow(string wantedClass, string wantedTitle)
  {
    HWND match = HWND.Null;

    PInvoke.EnumWindows((hWnd, _) =>
    {
      if (!string.IsNullOrEmpty(wantedClass) &&
          !string.Equals(GetWindowClass(hWnd), wantedClass, StringComparison.Ordinal))
      {
        return true;
      }

      if (!string.IsNullOrEmpty(wantedTitle) &&
          !string.Equals(GetWindowTitle(hWnd), wantedTitle, StringComparison.Ordinal))
      {
        return true;
      }

      match = hWnd;
      return false;
    }, default);

    return match;
  }

  private static unsafe string GetWindowClass(HWND hWnd)
  {
    Span<char> buffer = stackalloc char[256];
    fixed (char* p = buffer)
    {
      int length = PInvoke.GetClassName(hWnd, new PWSTR(p), buffer.Length);
      return length > 0 ? new string(buffer[..length]) : string.Empty;
    }
  }

  private static unsafe string GetWindowTitle(HWND hWnd)
  {
    Span<char> buffer = stackalloc char[512];
    fixed (char* p = buffer)
    {
      int length = PInvoke.GetWindowText(hWnd, new PWSTR(p), buffer.Length);
      return length > 0 ? new string(buffer[..length]) : string.Empty;
    }
  }
}
