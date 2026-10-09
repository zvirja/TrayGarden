using System.Collections.Generic;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TelegramToggleWindowHook;

public class PlantConfiguration : IUserConfiguration
{
    public static class MaximizeModes
    {
        public const string Off = "Off";
        public const string OnShow = "OnShow";
        public const string OnShowOrNormal = "OnShowOrNormal";
    }

    public static PlantConfiguration Instance { get; } = new();

    public IStringUserSetting ProcessName { get; set; }

    public IStringOptionUserSetting MaximizeMode { get; set; }

    public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
    {
        ProcessName = personalSettingsSteward.DeclareStringSetting("ProcessName", title: "Process Name", defaultValue: "", description: "If there are multiple processes, the oldest will be picked up");
        MaximizeMode = personalSettingsSteward.DeclareStringOptionSetting(
            "MaximizeMode",
            title: "Maximize window",
            defaultValue: MaximizeModes.Off,
            possibleOptions: new List<string> { MaximizeModes.Off, MaximizeModes.OnShow, MaximizeModes.OnShowOrNormal },
            description: "Off: plain minimize/restore.\nOnShow: maximize the window when the hotkey brings it back or switches to it.\nOnShowOrNormal: also maximize it if the hotkey finds it already visible but not maximized, before cycling maximize/minimize on further presses.");
    }

}