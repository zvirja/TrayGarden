using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
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
        return this._isEnabled;
      }
      set
      {
        if (value.Equals(this._isEnabled))
        {
          return;
        }
        this._isEnabled = value;
        this.OnPropertyChanged("IsEnabled");
        this.OnIsEnabledChanged(value);
      }
    }

    protected virtual void OnIsEnabledChanged(bool newvalue)
    {
      ServiceForPlantEnabledChanged handler = this.IsEnabledChanged;
      if (handler != null)
      {
        handler(this, newvalue);
      }
    }
  }
}