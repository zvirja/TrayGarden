using System;

using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace UngoogledChromiumUpdaterPlant;

public class PlantConfiguration : IUserConfiguration
{
  public static PlantConfiguration Instance { get; } = new();

  public IIntUserSetting CheckIntervalHours { get; set; }

  public TimeSpan ResolveCheckInterval()
  {
    int hours = CheckIntervalHours?.Value ?? 24;
    return TimeSpan.FromHours(Math.Max(1, hours));
  }

  public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
  {
    CheckIntervalHours = personalSettingsSteward.DeclareIntSetting(
      "CheckIntervalHours",
      "Check interval (hours)",
      24,
      "How often to look for a new release. A check also runs at startup.");
  }
}
