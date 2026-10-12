using JetBrains.Annotations;

using TrayGarden.UI;

using System.Windows.Input;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

/// <summary>
/// Opens plant user settings. Rendered as a dedicated header button rather than as a rare command row.
/// </summary>
public class ServiceForPlantUserSettingsVM(IUIManager uiManager, [NotNull] string serviceName, [NotNull] string description, [NotNull] ICommand action)
  : ServiceForPlantActionPerformVM(uiManager, serviceName, description, action);
