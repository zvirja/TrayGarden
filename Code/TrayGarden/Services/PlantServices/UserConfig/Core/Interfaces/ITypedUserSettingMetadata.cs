namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

public interface ITypedUserSettingMetadata<T> : IUserSettingMetadataBase
{
  T DefaultValue { get; }
}