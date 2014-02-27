#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
  public class ServiceForPlantActionPerformVM : ServiceForPlantVMBase
  {
    #region Fields

    protected ICommand _performServiceAction;

    #endregion

    #region Constructors and Destructors

    public ServiceForPlantActionPerformVM([NotNull] string serviceName, [NotNull] string description, [NotNull] ICommand action)
      : base(serviceName, description)
    {
      Assert.ArgumentNotNull(action, "action");
      this._performServiceAction = action;
    }

    #endregion

    #region Public Properties

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

    #endregion
  }
}