using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public class UserConfigServicePlantBox : ServicePlantBoxBase
{
  public IPersonalUserSettingsSteward SettingsSteward { get; set; }
}