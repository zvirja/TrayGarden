namespace TrayGarden.Features.RuntimeSettings.Provider
{
    public interface ISettingsStorage
    {
        IContainer GetRootContainer();
        void LoadSettings();
        bool SaveSettings();
    }
}