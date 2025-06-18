using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
  public class SinglePlantVM : INotifyPropertyChanged
  {
    protected string _description;

    protected string _name;

    protected ObservableCollection<ServiceForPlantVMBase> _servicesVM;

    public SinglePlantVM()
    {
      this.ServicesVM = new ObservableCollection<ServiceForPlantVMBase>();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public string Description
    {
      get
      {
        return this._description;
      }
      set
      {
        if (value == this._description)
        {
          return;
        }
        this._description = value;
        this.OnPropertyChanged("Description");
      }
    }

    [UsedImplicitly]
    public bool IsEnabled
    {
      get
      {
        return this.UnderlyingPlant != null ? this.UnderlyingPlant.IsEnabled : false;
      }
      set
      {
        this.UnderlyingPlant.IsEnabled = value;
        this.OnPropertyChanged("IsEnabled");
      }
    }

    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        if (value == this._name)
        {
          return;
        }
        this._name = value;
        this.OnPropertyChanged("Name");
      }
    }

    public ObservableCollection<ServiceForPlantVMBase> ServicesVM
    {
      get
      {
        return this._servicesVM;
      }
      set
      {
        if (Equals(value, this._servicesVM))
        {
          return;
        }
        this._servicesVM = value;
        this.OnPropertyChanged("ServicesVM");
      }
    }

    public IPlantEx UnderlyingPlant { get; set; }

    public virtual void InitPlantVMWithPlantEx([NotNull] IPlantEx underlyingPlant)
    {
      Assert.ArgumentNotNull(underlyingPlant, "underlyingPlant");
      this.UnderlyingPlant = underlyingPlant;
      this.Name = underlyingPlant.Plant.HumanSupportingName.GetValueOrDefault("<unspecified name>");
      this.Description = underlyingPlant.Plant.Description.GetValueOrDefault("<unspecified description>");
      underlyingPlant.EnabledChanged += this.UnderlyingPlant_EnabledChanged;
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    protected virtual void UnderlyingPlant_EnabledChanged(IPlantEx plantEx, bool newValue)
    {
      this.OnPropertyChanged("ServicesVM");
    }
  }
}