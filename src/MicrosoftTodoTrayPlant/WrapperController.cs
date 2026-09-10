using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using TrayGarden.Diagnostics;

using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace MicrosoftTodoTrayPlant;

internal enum WrapperState
{
  Idle,
  Launching,
  Visible,
  Hidden,
  Faulted,
  Detached
}

/// <summary>
/// Orchestrates the plant: launches the target app, disables its close button, and hides the
/// window to the tray on minimize / close / Alt+F4. The tray icon toggles it back.
/// </summary>
internal sealed class WrapperController
{
  public static WrapperController Instance { get; } = new();

  private static readonly TimeSpan LaunchTimeout = TimeSpan.FromSeconds(20);

  private readonly PlantConfiguration _config = PlantConfiguration.Instance;
  private readonly WrappedWindow _wrapped = new();

  private MinimizeWatcher _minimizeWatcher;
  private TargetKeyboardHook _keyboardHook;
  private DispatcherTimer _watchdog;
  private System.Diagnostics.Process _process;
  private WrapperState _state = WrapperState.Idle;

  private GlobalHotKey _hotKey;
  private bool _hotKeyDesired;
  private bool _hotKeyChangeSubscribed;

  private WrapperController()
  {
    AppDomain.CurrentDomain.ProcessExit += (_, _) => KillOnShutdown();
    AppDomain.CurrentDomain.UnhandledException += (_, _) => KillOnShutdown();
  }

  /// <summary>Register (or re-register) the global show/hide hotkey. Called when the plant is enabled.</summary>
  public void EnableHotKey()
  {
    Dispatch(() =>
    {
      _hotKeyDesired = true;

      if (!_hotKeyChangeSubscribed)
      {
        _config.EnableShowHotKey.ValueChanged += (_, _) => Dispatch(RegisterHotKeyFromConfig);
        _hotKeyChangeSubscribed = true;
      }

      RegisterHotKeyFromConfig();
    });
  }

  /// <summary>Drop the global hotkey. Called when the plant is disabled or the app exits.</summary>
  public void DisableHotKey()
  {
    Dispatch(() =>
    {
      _hotKeyDesired = false;
      _hotKey?.Dispose();
      _hotKey = null;
    });
  }

  private void RegisterHotKeyFromConfig()
  {
    _hotKey?.Dispose();
    _hotKey = null;

    if (!_hotKeyDesired || !_config.EnableShowHotKey.Value)
    {
      return;
    }

    _hotKey = new GlobalHotKey(HOT_KEY_MODIFIERS.MOD_ALT, VIRTUAL_KEY.VK_OEM_3, "Alt+`", ToggleVisibility);
  }

  /// <summary>Left-click on the tray icon, and the "Show / hide" menu entry.</summary>
  public void ToggleVisibility()
  {
    Dispatch(() =>
    {
      switch (_state)
      {
        case WrapperState.Visible:
          HideWindow();
          break;
        case WrapperState.Hidden:
          ShowWindowNow();
          break;
        case WrapperState.Launching:
          break;
        default:
          LaunchAndWrap();
          break;
      }
    });
  }

  /// <summary>"Close To Do" menu entry: stop managing the window and terminate the process.</summary>
  public void CloseTarget()
  {
    Dispatch(() =>
    {
      System.Diagnostics.Process toKill = _process;
      _process = null;

      // Stop the hooks first, then kill: the window dies with the process, no need to restore its
      // close button or briefly flash it back into view.
      Teardown(releaseWindow: false);
      TryKill(toKill);

      _state = WrapperState.Idle;
    });
  }

  /// <summary>App shutting down: drop the hotkey and make sure the wrapped app goes with us.</summary>
  public void KillOnShutdown()
  {
    Dispatch(() =>
    {
      _hotKeyDesired = false;
      _hotKey?.Dispose();
      _hotKey = null;

      System.Diagnostics.Process toKill = _process;
      _process = null;
      Teardown(releaseWindow: false);
      TryKill(toKill);

      _state = WrapperState.Idle;
    });
  }

  /// <summary>Plant disabled: re-enable the close button and leave the app running as a normal window.</summary>
  public void DetachAndRelease()
  {
    Dispatch(() =>
    {
      if (_state == WrapperState.Idle || _state == WrapperState.Detached)
      {
        return;
      }

      System.Diagnostics.Process current = _process;
      Teardown(releaseWindow: true);
      _process = null;

      if (current != null)
      {
        try
        {
          current.Exited -= OnProcessExited;
        }
        catch
        {
          // ignored
        }
      }

      _state = WrapperState.Detached;
    });
  }

  private void LaunchAndWrap()
  {
    _state = WrapperState.Launching;

    if (TodoLauncher.FindProcess(_config) == null)
    {
      try
      {
        TodoLauncher.StartProcess(_config);
      }
      catch (Exception ex)
      {
        Log.For(this).Error(ex, "Failed to launch target app.");
        _state = WrapperState.Faulted;
        return;
      }
    }

    Task.Run(() => TodoLauncher.WaitForTarget(_config, LaunchTimeout))
      .ContinueWith(t =>
      {
        WrappedTarget target = t.Result;
        Dispatch(() =>
        {
          if (_state != WrapperState.Launching)
          {
            return;
          }

          if (target == null)
          {
            _state = WrapperState.Faulted;
            return;
          }

          AttachTarget(target);
        });
      }, TaskScheduler.Default);
  }

  private void AttachTarget(WrappedTarget target)
  {
    _process = target.Process;

    try
    {
      _process.EnableRaisingEvents = true;
      _process.Exited += OnProcessExited;
    }
    catch (Exception ex)
    {
      Log.For(this).Warning(ex, "Could not subscribe to target process exit.");
    }

    _wrapped.Wrap(target.FrameWindow);

    uint threadId = PInvoke.GetWindowThreadProcessId(target.FrameWindow, out uint processId);
    _minimizeWatcher = new MinimizeWatcher(target.FrameWindow, processId, threadId, OnTargetMinimizeStart);

    _keyboardHook = new TargetKeyboardHook(() => _wrapped.Handle, () => _config.HideOnEscape.Value, HideWindow);
    _keyboardHook.Install();

    StartWatchdog();

    if (_config.StartHidden.Value)
    {
      _wrapped.Hide();
      _state = WrapperState.Hidden;
    }
    else
    {
      _wrapped.Show();
      _state = WrapperState.Visible;
    }
  }

  private void OnTargetMinimizeStart()
  {
    Dispatch(() =>
    {
      if (_state == WrapperState.Visible)
      {
        HideWindow();
      }
    });
  }

  private void HideWindow()
  {
    Dispatch(() =>
    {
      if (_state != WrapperState.Visible)
      {
        return;
      }

      _wrapped.Hide();
      _state = WrapperState.Hidden;
    });
  }

  private void ShowWindowNow()
  {
    if (_state != WrapperState.Hidden)
    {
      return;
    }

    _wrapped.Show();
    _state = WrapperState.Visible;
  }

  private void OnProcessExited(object sender, EventArgs e)
  {
    Dispatch(() =>
    {
      Log.For(this).Information("Target process exited on its own; tearing down wrapper.");
      Teardown(releaseWindow: false);
      _process = null;
      _state = WrapperState.Idle;
    });
  }

  private void StartWatchdog()
  {
    _watchdog = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
    _watchdog.Tick += (_, _) =>
    {
      if (_wrapped.Exists)
      {
        return;
      }

      if (_state == WrapperState.Visible || _state == WrapperState.Hidden)
      {
        Log.For(this).Warning("Wrapped window disappeared; tearing down.");
        Teardown(releaseWindow: false);
        _process = null;
        _state = WrapperState.Idle;
      }
    };
    _watchdog.Start();
  }

  private void Teardown(bool releaseWindow)
  {
    _watchdog?.Stop();
    _watchdog = null;

    _minimizeWatcher?.Dispose();
    _minimizeWatcher = null;

    _keyboardHook?.Dispose();
    _keyboardHook = null;

    if (releaseWindow)
    {
      _wrapped.Release();
    }
  }

  private void TryKill(System.Diagnostics.Process process)
  {
    if (process == null)
    {
      return;
    }

    try
    {
      process.Exited -= OnProcessExited;
      if (!process.HasExited)
      {
        process.Kill(entireProcessTree: true);
      }
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Failed to terminate target process.");
    }
  }

  private static void Dispatch(Action action)
  {
    Dispatcher dispatcher = Application.Current?.Dispatcher;
    if (dispatcher == null || dispatcher.CheckAccess())
    {
      action();
    }
    else
    {
      dispatcher.Invoke(action);
    }
  }
}
