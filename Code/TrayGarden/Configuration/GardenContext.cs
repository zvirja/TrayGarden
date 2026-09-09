using TrayGarden.RuntimeSettings;
using TrayGarden.UI.Common;

namespace TrayGarden.Configuration;

/// <summary>
/// Holds the few collaborators that objects created by WPF (XAML-instantiated
/// template selectors) and static configuration holders cannot receive through
/// their constructors. Populated once by the application bootstrapper before the
/// startup pipeline runs. Everything else uses constructor injection.
/// </summary>
public static class GardenContext
{
  public static IRuntimeSettingsManager RuntimeSettings { get; internal set; }

  public static IDataTemplateSelector ServiceForPlantTemplateSelector { get; internal set; }
}
