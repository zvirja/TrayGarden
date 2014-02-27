#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
  public class PlantsConfigVM : INotifyPropertyChanged
  {
    #region Fields

    protected ObservableCollection<SinglePlantVM> _plantVMs;

    #endregion

    #region Constructors and Destructors

    public PlantsConfigVM()
    {
      this.PlantVMs = new ObservableCollection<SinglePlantVM>();
    }

    #endregion

    #region Public Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Public Properties

    public ObservableCollection<SinglePlantVM> PlantVMs
    {
      get
      {
        return this._plantVMs;
      }
      set
      {
        if (Equals(value, this._plantVMs))
        {
          return;
        }
        this._plantVMs = value;
        this.OnPropertyChanged("PlantVMs");
      }
    }

    #endregion

    #region Methods

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion
  }
}