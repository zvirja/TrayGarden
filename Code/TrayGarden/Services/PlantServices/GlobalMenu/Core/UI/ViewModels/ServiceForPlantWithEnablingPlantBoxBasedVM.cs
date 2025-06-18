using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
  [UsedImplicitly]
  public class ServiceForPlantWithEnablingPlantBoxBasedVM : ServiceForPlantWithEnablingVM, IDisposable
  {
    public ServiceForPlantWithEnablingPlantBoxBasedVM(
      [NotNull] string serviceName,
      [NotNull] string description,
      ServicePlantBoxBase plantBox)
      : base(serviceName, description)
    {
      this.AssignedPlantBox = plantBox;
      this.AssignedPlantBox.IsEnabledChanged += this.AssignedPlantBox_IsEnabledChanged;
    }

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

    protected ServicePlantBoxBase AssignedPlantBox { get; set; }

    public virtual void Dispose()
    {
      this.AssignedPlantBox.IsEnabledChanged -= this.AssignedPlantBox_IsEnabledChanged;
    }

    protected virtual void AssignedPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newValue)
    {
      this.OnPropertyChanged("IsEnabled");
    }
  }
}