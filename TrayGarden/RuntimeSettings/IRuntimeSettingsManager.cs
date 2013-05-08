namespace TrayGarden.RuntimeSettings
{
    public interface IRuntimeSettingsManager
    {
        ISettingsBox SystemSettings { get; }
        ISettingsBox OtherSettings { get; }
        bool SaveNow();
    }

}