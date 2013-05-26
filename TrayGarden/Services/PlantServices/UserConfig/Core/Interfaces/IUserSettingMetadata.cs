using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingMetadata
    {
        string Name { get;  }
        string DefaultValue { get;  }
        UserSettingValueType SettingValueType { get;  }
        object AdditionalParams { get; }
    }
}
