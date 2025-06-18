using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

[UsedImplicitly]
public class ServiceForPlantWithEnablingVM : ServiceForPlantVMBase
{
  protected bool _isEnabled;

  public ServiceForPlantWithEnablingVM([NotNull] string serviceName, [NotNull] string description)
    : base(serviceName, description)
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

  protected virtual void OnIsEnabledChanged(bool newvalue)
  {
    ServiceForPlantEnabledChanged handler = IsEnabledChanged;
    if (handler != null)
    {
      handler(this, newvalue);
    }
  }
}