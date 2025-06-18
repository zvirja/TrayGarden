namespace TrayGarden.RuntimeSettings.Provider;

public interface ISettingsStorage
{
  IContainer GetRootContainer();

  void LoadSettings();

  bool SaveSettings();
}