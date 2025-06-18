using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

public class ServiceForPlantActionPerformVM : ServiceForPlantVMBase
{
  protected ICommand _performServiceAction;

  public ServiceForPlantActionPerformVM([NotNull] string serviceName, [NotNull] string description, [NotNull] ICommand action)
    : base(serviceName, description)
  {
    Assert.ArgumentNotNull(action, "action");
    this._performServiceAction = action;
  }

  public ICommand PerformServiceAction
  {
    get
    {
      return this._performServiceAction;
    }
    set
    {
      if (Equals(value, this._performServiceAction))
      {
        return;
      }
      this._performServiceAction = value;
      this.OnPropertyChanged("PerformServiceAction");
    }
  }
}