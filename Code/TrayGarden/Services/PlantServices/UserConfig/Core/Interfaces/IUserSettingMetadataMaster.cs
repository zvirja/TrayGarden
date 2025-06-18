namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

public interface IUserSettingMetadataMaster<T> : ITypedUserSettingMetadata<T>
{
  void Initialize(string name, string title, T defaultValue, string description, object additionalParams, IUserSettingHallmark hallmark);
}