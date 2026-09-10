using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

public class ServiceForPlantActionPerformVM : ServiceForPlantVMBase
{
  private ICommand _performServiceAction;

  public ServiceForPlantActionPerformVM(IUIManager uiManager, [NotNull] string serviceName, [NotNull] string description, [NotNull] ICommand action)
    : base(uiManager, serviceName, description)
  {
    Assert.ArgumentNotNull(action, "action");
    _performServiceAction = action;
  }

  public ICommand PerformServiceAction
  {
    get
    {
      return _performServiceAction;
    }
    set
    {
      if (Equals(value, _performServiceAction))
      {
        return;
      }
      _performServiceAction = value;
      OnPropertyChanged("PerformServiceAction");
    }
  }
}