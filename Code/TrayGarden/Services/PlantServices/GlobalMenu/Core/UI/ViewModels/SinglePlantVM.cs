using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

public class SinglePlantVM : INotifyPropertyChanged
{
  protected string _description;

  protected string _name;

  protected ObservableCollection<ServiceForPlantVMBase> _servicesVM;

  public SinglePlantVM()
  {
    ServicesVM = new ObservableCollection<ServiceForPlantVMBase>();
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public string Description
  {
    get
    {
      return _description;
    }
    set
    {
      if (value == _description)
      {
        return;
      }
      _description = value;
      OnPropertyChanged("Description");
    }
  }

  [UsedImplicitly]
  public bool IsEnabled
  {
    get
    {
      return UnderlyingPlant != null ? UnderlyingPlant.IsEnabled : false;
    }
    set
    {
      UnderlyingPlant.IsEnabled = value;
      OnPropertyChanged("IsEnabled");
    }
  }

  public string Name
  {
    get
    {
      return _name;
    }
    set
    {
      if (value == _name)
      {
        return;
      }
      _name = value;
      OnPropertyChanged("Name");
    }
  }

  public ObservableCollection<ServiceForPlantVMBase> ServicesVM
  {
    get
    {
      return _servicesVM;
    }
    set
    {
      if (Equals(value, _servicesVM))
      {
        return;
      }
      _servicesVM = value;
      OnPropertyChanged("ServicesVM");
    }
  }

  public IPlantEx UnderlyingPlant { get; set; }

  public virtual void InitPlantVMWithPlantEx([NotNull] IPlantEx underlyingPlant)
  {
    Assert.ArgumentNotNull(underlyingPlant, "underlyingPlant");
    UnderlyingPlant = underlyingPlant;
    Name = underlyingPlant.Plant.HumanSupportingName.GetValueOrDefault("<unspecified name>");
    Description = underlyingPlant.Plant.Description.GetValueOrDefault("<unspecified description>");
    underlyingPlant.EnabledChanged += UnderlyingPlant_EnabledChanged;
  }

  [NotifyPropertyChangedInvocator]
  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler handler = PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  protected virtual void UnderlyingPlant_EnabledChanged(IPlantEx plantEx, bool newValue)
  {
    OnPropertyChanged("ServicesVM");
  }
}