using System;
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
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
  public class ServiceForPlantVMBase : INotifyPropertyChanged
  {
    protected string _description;

    protected string _serviceName;

    protected ICommand _showDescription;

    public ServiceForPlantVMBase([NotNull] string serviceName, [NotNull] string description)
    {
      Assert.ArgumentNotNullOrEmpty(serviceName, "serviceName");
      Assert.ArgumentNotNullOrEmpty(description, "description");
      this.ServiceName = serviceName;
      this.Description = description;
      this.ShowDescription = new RelayCommand(this.ShowDescriptionAction, true);
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

    public object Luggage { get; set; }

    public string ServiceName
    {
      get
      {
        return this._serviceName;
      }
      set
      {
        if (value == this._serviceName)
        {
          return;
        }
        this._serviceName = value;
        this.OnPropertyChanged("ServiceName");
      }
    }

    public ICommand ShowDescription
    {
      get
      {
        return this._showDescription;
      }
      set
      {
        if (Equals(value, this._showDescription))
        {
          return;
        }
        this._showDescription = value;
        this.OnPropertyChanged("ShowDescription");
      }
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

    protected virtual void ShowDescriptionAction(object o)
    {
      HatcherGuide<IUIManager>.Instance.OKMessageBox(this.ServiceName, this.Description, MessageBoxImage.Question);
    }
  }
}