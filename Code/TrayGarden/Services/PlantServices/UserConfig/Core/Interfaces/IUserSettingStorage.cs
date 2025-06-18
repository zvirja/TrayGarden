namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

public interface IUserSettingStorage<T>
{
  T ReadValue(string key, T defaultValue);

  void WriteValue(string key, T value);
}