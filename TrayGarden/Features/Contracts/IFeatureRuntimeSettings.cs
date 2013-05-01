using TrayGarden.Features.RuntimeSettings;

namespace TrayGarden.Features.Contracts
{
    public interface IFeatureRuntimeSettings
    {
        ISettingsBox SystemSettings { get; }
        ISettingsBox OtherSettings { get; }
        bool SaveNow();
    }

}