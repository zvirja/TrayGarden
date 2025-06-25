using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TelegramToggleWindowHook;

public class PlantConfiguration : IUserConfiguration
{
    public static PlantConfiguration Instance { get; } = new();

    public IStringUserSetting ProcessName { get; set; }
    
    public IBoolUserSetting ShowMaximized { get; set; }

    public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
    {
        ProcessName = personalSettingsSteward.DeclareStringSetting("ProcessName", title: "Process Name", defaultValue: "", description: "If there are multiple processes, the oldest will be picked up");
        ShowMaximized = personalSettingsSteward.DeclareBoolSetting("ShowMaximized", title: "Maximize window on restore", defaultValue: false);
    }

}