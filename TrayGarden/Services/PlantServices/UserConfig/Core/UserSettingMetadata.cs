using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public class UserSettingMetadata : IUserSettingMetadataMaster
    {
        public virtual string Name { get; protected set; }
        public virtual string DefaultValue { get; protected set; }
        public virtual UserSettingValueType SettingValueType { get; protected set; }
        public virtual object AdditionalParams { get; protected set; }

        public virtual void Initialize(string name, UserSettingValueType valueType, string defaultValue, object additionalParams)
        {
            Name = name;
            DefaultValue = defaultValue;
            SettingValueType = valueType;
            AdditionalParams = additionalParams;
        }
    }
}