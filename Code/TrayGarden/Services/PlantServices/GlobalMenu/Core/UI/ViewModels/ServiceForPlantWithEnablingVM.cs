#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
  [UsedImplicitly]
  public class ServiceForPlantWithEnablingVM : ServiceForPlantVMBase
  {
    #region Fields

    protected bool _isEnabled;

    #endregion

    #region Constructors and Destructors

    public ServiceForPlantWithEnablingVM([NotNull] string serviceName, [NotNull] string description)
      : base(serviceName, description)
    {
    }

    #endregion

    #region Delegates

    public delegate void ServiceForPlantEnabledChanged(ServiceForPlantWithEnablingVM sender, bool newValue);

    #endregion

    #region Public Events

    public event ServiceForPlantEnabledChanged IsEnabledChanged;

    #endregion

    #region Public Properties

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

    #endregion

    #region Methods

    protected virtual void OnIsEnabledChanged(bool newvalue)
    {
      ServiceForPlantEnabledChanged handler = this.IsEnabledChanged;
      if (handler != null)
      {
        handler(this, newvalue);
      }
    }

    #endregion
  }
}