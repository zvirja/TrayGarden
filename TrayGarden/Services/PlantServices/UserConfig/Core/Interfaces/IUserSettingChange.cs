using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingChange
    {
        IUserSetting NewUserSetting { get; }

        [CanBeNull]
        IUserSetting OldUserSetting { get; }
    }
}
