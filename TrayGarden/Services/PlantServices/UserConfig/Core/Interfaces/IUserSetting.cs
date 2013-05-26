using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSetting
    {
        int? IntValue { get; set; }
        bool? BoolValue { get; set; }
        string StringValue { get; set; }
        string StringOptionValue { get; set; }

        /// <summary>
        /// Leave a way to extend service. By default this value always is null.
        /// </summary>
        string CustomTypeValue { get; set; }

        string Name { get; }

        UserSettingValueType ValueType { get; }
        IUserSettingMetadata Metadata { get; }
    }
}
