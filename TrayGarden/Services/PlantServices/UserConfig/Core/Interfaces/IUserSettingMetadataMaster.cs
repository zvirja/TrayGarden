using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingMetadataMaster : IUserSettingMetadata
    {
        void Initialize(string name,UserSettingValueType valueType,string defaultValue,object additionalParams);
    }
}