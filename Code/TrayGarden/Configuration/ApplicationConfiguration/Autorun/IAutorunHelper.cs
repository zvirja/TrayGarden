namespace TrayGarden.Configuration.ApplicationConfiguration.Autorun;

public interface IAutorunHelper
{
  bool IsAddedToAutorun { get; }

  bool SetNewAutorunValue(bool runAtStartup);
}