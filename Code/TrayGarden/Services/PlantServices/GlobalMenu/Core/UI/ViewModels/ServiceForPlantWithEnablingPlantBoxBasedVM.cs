using System;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

[UsedImplicitly]
public class ServiceForPlantWithEnablingPlantBoxBasedVM : ServiceForPlantWithEnablingVM, IDisposable
{
  public ServiceForPlantWithEnablingPlantBoxBasedVM(
    [NotNull] string serviceName,
    [NotNull] string description,
    ServicePlantBoxBase plantBox)
    : base(serviceName, description)
  {
    AssignedPlantBox = plantBox;
    AssignedPlantBox.IsEnabledChanged += AssignedPlantBox_IsEnabledChanged;
  }

  [UsedImplicitly]
  public override bool IsEnabled
  {
    get
    {
      return AssignedPlantBox.IsEnabled;
    }
    set
    {
      if (value.Equals(AssignedPlantBox.IsEnabled))
      {
        return;
      }
      AssignedPlantBox.IsEnabled = value;
      OnPropertyChanged("IsEnabled");
      OnIsEnabledChanged(value);
    }
  }

  protected ServicePlantBoxBase AssignedPlantBox { get; set; }

  public virtual void Dispose()
  {
    AssignedPlantBox.IsEnabledChanged -= AssignedPlantBox_IsEnabledChanged;
  }

  protected virtual void AssignedPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newValue)
  {
    OnPropertyChanged("IsEnabled");
  }
}