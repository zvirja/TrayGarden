using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

public class PlantsConfigVM : INotifyPropertyChanged
{
  protected ObservableCollection<SinglePlantVM> _plantVMs;

  public PlantsConfigVM()
  {
    this.PlantVMs = new ObservableCollection<SinglePlantVM>();
  }

  public event PropertyChangedEventHandler PropertyChanged;

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

  [NotifyPropertyChangedInvocator]
  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler handler = this.PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}