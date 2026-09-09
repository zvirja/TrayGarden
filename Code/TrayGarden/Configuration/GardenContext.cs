using TrayGarden.RuntimeSettings;
using TrayGarden.UI;
using TrayGarden.UI.Common;

namespace TrayGarden.Configuration;

/// <summary>
/// Holds the few collaborators that objects created outside the container can
/// not receive through their constructors: WPF (XAML-instantiated) types, static
/// configuration holders, and plants loaded by reflection. Populated once by the
/// application bootstrapper before the startup pipeline runs. Everything created
/// by the container uses constructor injection instead.
/// </summary>
public static class GardenContext
{
  public static IRuntimeSettingsManager RuntimeSettings { get; internal set; }

  public static IDataTemplateSelector ServiceForPlantTemplateSelector { get; internal set; }

  public static IUIManager UIManager { get; internal set; }
}
