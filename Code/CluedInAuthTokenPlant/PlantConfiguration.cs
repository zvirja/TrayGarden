using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace CluedInAuthTokenPlant;

public class PlantConfiguration: IUserConfiguration
{
    public static PlantConfiguration Instance { get; } = new();
    
    public IStringUserSetting AuthUrl { get; set; }
    
    public IStringUserSetting AuthOrgName { get; set; }
    
    public IStringUserSetting AuthUser { get; set; }
    
    public IStringUserSetting AuthPassword { get; set; }
    
    public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
    {
        AuthUrl = personalSettingsSteward.DeclareStringSetting("AuthUrl", "Auth URL", "http://localhost:9001/connect/token");
        AuthOrgName = personalSettingsSteward.DeclareStringSetting("AuthOrgName", "Auth Organization", "foobar");
        AuthUser = personalSettingsSteward.DeclareStringSetting("AuthUser", "Auth User", "admin@foobar.com");
        AuthPassword = personalSettingsSteward.DeclareStringSetting("AuthPassword", "Auth Password", "Foobar23!");
    }
}