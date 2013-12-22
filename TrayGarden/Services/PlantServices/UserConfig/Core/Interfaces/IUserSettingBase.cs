using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface IUserSettingBase
  {
    event UserSettingValueChanged Changed;
    string Name { get; }
    UserSettingValueType ValueType { get; }
    IUserSettingMetadata Metadata { get; }
    void ResetToDefault();
  }
}