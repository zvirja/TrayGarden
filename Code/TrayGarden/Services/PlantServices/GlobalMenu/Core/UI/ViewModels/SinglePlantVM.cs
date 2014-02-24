using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
    public class SinglePlantVM: INotifyPropertyChanged
    {
        protected string _name;
        protected string _description;
        protected ObservableCollection<ServiceForPlantVMBase> _servicesVM;

        public IPlantEx UnderlyingPlant { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        [UsedImplicitly]
        public bool IsEnabled
        {
            get { return UnderlyingPlant!=null?UnderlyingPlant.IsEnabled:false; }
            set
            {
                UnderlyingPlant.IsEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public ObservableCollection<ServiceForPlantVMBase> ServicesVM
        {
            get { return _servicesVM; }
            set
            {
                if (Equals(value, _servicesVM)) return;
                _servicesVM = value;
                OnPropertyChanged("ServicesVM");
            }
        }

        public SinglePlantVM()
        {
            ServicesVM = new ObservableCollection<ServiceForPlantVMBase>();
        }

        public virtual void InitPlantVMWithPlantEx([NotNull] IPlantEx underlyingPlant)
        {
            Assert.ArgumentNotNull(underlyingPlant, "underlyingPlant");
            UnderlyingPlant = underlyingPlant;
            Name = underlyingPlant.Plant.HumanSupportingName.GetValueOrDefault("<unspecified name>");
            Description = underlyingPlant.Plant.Description.GetValueOrDefault("<unspecified description>");
            underlyingPlant.EnabledChanged += UnderlyingPlant_EnabledChanged;
        }

        protected virtual void UnderlyingPlant_EnabledChanged(IPlantEx plantEx, bool newValue)
        {
            OnPropertyChanged("ServicesVM");
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
