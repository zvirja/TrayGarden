using JetBrains.Annotations;

using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

[UsedImplicitly]
public class ServiceForPlantWithEnablingVM : ServiceForPlantVMBase
{
  private bool _isEnabled;

  public ServiceForPlantWithEnablingVM(IUIManager uiManager, [NotNull] string serviceName, [NotNull] string description)
    : base(uiManager, serviceName, description)
  {
  }

  public delegate void ServiceForPlantEnabledChanged(ServiceForPlantWithEnablingVM sender, bool newValue);

  public event ServiceForPlantEnabledChanged IsEnabledChanged;

  [UsedImplicitly]
  public virtual bool IsEnabled
  {
    get
    {
      return _isEnabled;
    }
    set
    {
      if (value.Equals(_isEnabled))
      {
        return;
      }
      _isEnabled = value;
      OnPropertyChanged("IsEnabled");
      OnIsEnabledChanged(value);
    }
  }

  protected void OnIsEnabledChanged(bool newvalue)
  {
    ServiceForPlantEnabledChanged handler = IsEnabledChanged;
    if (handler != null)
    {
      handler(this, newvalue);
    }
  }
}