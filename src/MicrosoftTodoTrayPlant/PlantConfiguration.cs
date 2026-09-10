using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace MicrosoftTodoTrayPlant;

public class PlantConfiguration : IUserConfiguration
{
  public static PlantConfiguration Instance { get; } = new();

  public IStringUserSetting LaunchCommand { get; set; }

  public IStringUserSetting ProcessName { get; set; }

  public IStringUserSetting FrameWindowClass { get; set; }

  public IStringUserSetting FrameWindowTitle { get; set; }

  public IBoolUserSetting StartHidden { get; set; }

  public IBoolUserSetting HideOnEscape { get; set; }

  public IBoolUserSetting EnableShowHotKey { get; set; }

  public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
  {
    LaunchCommand = personalSettingsSteward.DeclareStringSetting(
      "LaunchCommand",
      "Launch command",
      @"shell:AppsFolder\Microsoft.Todos_8wekyb3d8bbwe!App",
      "Command used to start the target app. Run through the shell (UseShellExecute), so a 'shell:AppsFolder\\...' AUMID or a plain exe path both work.");

    ProcessName = personalSettingsSteward.DeclareStringSetting(
      "ProcessName",
      "Process name",
      "Todo",
      "Process name (without .exe) used to find the running app and to terminate it from the 'Close' menu entry.");

    FrameWindowClass = personalSettingsSteward.DeclareStringSetting(
      "FrameWindowClass",
      "Frame window class",
      "ApplicationFrameWindow",
      "Win32 class of the top-level window to manage. For packaged / UWP apps this is ApplicationFrameWindow.");

    FrameWindowTitle = personalSettingsSteward.DeclareStringSetting(
      "FrameWindowTitle",
      "Frame window title",
      "Microsoft To Do",
      "Exact title of the top-level window to manage. Leave empty to match the first window of the given class.");

    StartHidden = personalSettingsSteward.DeclareBoolSetting(
      "StartHidden",
      "Start hidden in tray",
      false,
      "After launching the app, hide it to the tray immediately instead of showing the window.");

    HideOnEscape = personalSettingsSteward.DeclareBoolSetting(
      "HideOnEscape",
      "Hide on Esc",
      true,
      "Hide the window to the tray when Esc is pressed while it is the active window. The key press is swallowed, so the app does not also see it.");

    EnableShowHotKey = personalSettingsSteward.DeclareBoolSetting(
      "EnableShowHotKey",
      "Alt+` show / hide hotkey",
      true,
      "Register a system-wide Alt+` (Alt+backtick) hotkey that toggles the To Do window.");
  }
}
