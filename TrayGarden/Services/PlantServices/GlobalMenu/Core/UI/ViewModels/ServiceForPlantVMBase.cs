﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;
using TrayGarden.UI.Common;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
    public class ServiceForPlantVMBase: INotifyPropertyChanged
    {
        protected ICommand _showDescription;
        protected string _serviceName;
        protected string _description;

        public object Luggage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
       
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

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                if (value == _serviceName) return;
                _serviceName = value;
                OnPropertyChanged("ServiceName");
            }
        }

        public ICommand ShowDescription
        {
            get { return _showDescription; }
            set
            {
                if (Equals(value, _showDescription)) return;
                _showDescription = value;
                OnPropertyChanged("ShowDescription");
            }
        }


        public ServiceForPlantVMBase([NotNull] string serviceName, [NotNull] string description)
        {
            Assert.ArgumentNotNullOrEmpty(serviceName, "serviceName");
            Assert.ArgumentNotNullOrEmpty(description, "description");
            ServiceName = serviceName;
            Description = description;
            ShowDescription = new RelayCommand(ShowDescriptionAction, true);
        }

        


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void ShowDescriptionAction(object o)
        {
            HatcherGuide<IUIManager>.Instance.OKMessageBox(ServiceName, Description, MessageBoxImage.Question);
        }
    }
}
