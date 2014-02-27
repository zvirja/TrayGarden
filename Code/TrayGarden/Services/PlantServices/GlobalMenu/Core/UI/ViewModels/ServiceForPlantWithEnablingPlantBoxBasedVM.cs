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
  public class ServiceForPlantWithEnablingPlantBoxBasedVM : ServiceForPlantWithEnablingVM, IDisposable
  {
    #region Constructors and Destructors

    public ServiceForPlantWithEnablingPlantBoxBasedVM(
      [NotNull] string serviceName,
      [NotNull] string description,
      ServicePlantBoxBase plantBox)
      : base(serviceName, description)
    {
      this.AssignedPlantBox = plantBox;
      this.AssignedPlantBox.IsEnabledChanged += this.AssignedPlantBox_IsEnabledChanged;
    }

    #endregion

    #region Public Properties

    [UsedImplicitly]
    public override bool IsEnabled
    {
      get
      {
        return this.AssignedPlantBox.IsEnabled;
      }
      set
      {
        if (value.Equals(this.AssignedPlantBox.IsEnabled))
        {
          return;
        }
        this.AssignedPlantBox.IsEnabled = value;
        this.OnPropertyChanged("IsEnabled");
        this.OnIsEnabledChanged(value);
      }
    }

    #endregion

    #region Properties

    protected ServicePlantBoxBase AssignedPlantBox { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual void Dispose()
    {
      this.AssignedPlantBox.IsEnabledChanged -= this.AssignedPlantBox_IsEnabledChanged;
    }

    #endregion

    #region Methods

    protected virtual void AssignedPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newValue)
    {
      this.OnPropertyChanged("IsEnabled");
    }

    #endregion
  }
}