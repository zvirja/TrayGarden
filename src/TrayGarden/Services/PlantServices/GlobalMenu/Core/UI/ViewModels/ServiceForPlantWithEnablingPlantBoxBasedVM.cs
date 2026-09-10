using System;
using JetBrains.Annotations;

using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

[UsedImplicitly]
public class ServiceForPlantWithEnablingPlantBoxBasedVM : ServiceForPlantWithEnablingVM, IDisposable
{
  public ServiceForPlantWithEnablingPlantBoxBasedVM(
    IUIManager uiManager,
    [NotNull] string serviceName,
    [NotNull] string description,
    ServicePlantBoxBase plantBox)
    : base(uiManager, serviceName, description)
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

  private ServicePlantBoxBase AssignedPlantBox { get; set; }

  public void Dispose()
  {
    AssignedPlantBox.IsEnabledChanged -= AssignedPlantBox_IsEnabledChanged;
  }

  private void AssignedPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newValue)
  {
    OnPropertyChanged("IsEnabled");
  }
}