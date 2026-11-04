using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace DevDashboardPlant;

public class PlantConfiguration : IUserConfiguration
{
  public static PlantConfiguration Instance { get; } = new();

  public IStringUserSetting Url { get; private set; }

  public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
  {
    Url = personalSettingsSteward.DeclareStringSetting(
      "Url",
      "Website URL",
      "https://dev-dashboard.puddle.vpn.zvirja.com/",
      "Website shown in the app window. Changing it navigates an already loaded window to the new address.");
  }
}
