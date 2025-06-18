namespace TrayGarden.RuntimeSettings;

public interface IRuntimeSettingsManager
{
  ISettingsBox OtherSettings { get; }

  ISettingsBox SystemSettings { get; }

  bool SaveNow(bool force);
}