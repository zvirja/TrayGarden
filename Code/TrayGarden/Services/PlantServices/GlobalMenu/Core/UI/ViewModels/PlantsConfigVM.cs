﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

public class PlantsConfigVM : INotifyPropertyChanged
{
  protected ObservableCollection<SinglePlantVM> _plantVMs;

  public PlantsConfigVM()
  {
    PlantVMs = new ObservableCollection<SinglePlantVM>();
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public ObservableCollection<SinglePlantVM> PlantVMs
  {
    get
    {
      return _plantVMs;
    }
    set
    {
      if (Equals(value, _plantVMs))
      {
        return;
      }
      _plantVMs = value;
      OnPropertyChanged("PlantVMs");
    }
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
}