using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

public class SinglePlantVM : INotifyPropertyChanged
{
  private string _description;

  private string _name;

  private ObservableCollection<ServiceForPlantVMBase> _servicesVM;

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
      if (_servicesVM != null)
      {
        _servicesVM.CollectionChanged -= ServicesVM_CollectionChanged;
      }
      _servicesVM = value;
      if (_servicesVM != null)
      {
        _servicesVM.CollectionChanged += ServicesVM_CollectionChanged;
      }
      OnServicesChanged();
    }
  }

  public IEnumerable<ServiceForPlantWithEnablingVM> ToggleServices => ServicesVM.OfType<ServiceForPlantWithEnablingVM>().ToList();

  public bool HasToggleServices => ServicesVM.OfType<ServiceForPlantWithEnablingVM>().Any();

  public ServiceForPlantUserSettingsVM SettingsService => ServicesVM.OfType<ServiceForPlantUserSettingsVM>().FirstOrDefault();

  public bool HasRareCommands => RareCommands.Any();

  public IEnumerable<ServiceForPlantActionPerformVM> RareCommands =>
    ServicesVM.OfType<ServiceForPlantActionPerformVM>().Where(x => x is not ServiceForPlantUserSettingsVM).ToList();

  public IPlantEx UnderlyingPlant { get; set; }

  public void InitPlantVMWithPlantEx([NotNull] IPlantEx underlyingPlant)
  {
    Assert.ArgumentNotNull(underlyingPlant, "underlyingPlant");
    UnderlyingPlant = underlyingPlant;
    Name = underlyingPlant.Plant.HumanSupportingName.GetValueOrDefault("<unspecified name>");
    Description = underlyingPlant.Plant.Description.GetValueOrDefault("<unspecified description>");
    underlyingPlant.EnabledChanged += UnderlyingPlant_EnabledChanged;
  }

  [NotifyPropertyChangedInvocator]
  private void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler handler = PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  private void OnServicesChanged()
  {
    OnPropertyChanged("ServicesVM");
    OnPropertyChanged("ToggleServices");
    OnPropertyChanged("HasToggleServices");
    OnPropertyChanged("SettingsService");
    OnPropertyChanged("RareCommands");
    OnPropertyChanged("HasRareCommands");
  }

  private void ServicesVM_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    OnServicesChanged();
  }

  private void UnderlyingPlant_EnabledChanged(IPlantEx plantEx, bool newValue)
  {
    OnServicesChanged();
  }
}