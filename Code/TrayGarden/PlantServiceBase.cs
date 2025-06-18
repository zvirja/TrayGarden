using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services;

public abstract class PlantServiceBase<TPlantLuggageType> : IService
  where TPlantLuggageType : class
{
  protected static readonly string AllServiceSettingsContainerName = "PlantServices";

  protected bool? _isActuallyEnabled;

  protected ISettingsBox _serviceSettingsBox;

  protected PlantServiceBase(string serviceName, string luggageName)
  {
    this.ServiceName = serviceName;
    this.LuggageName = luggageName;
  }

  public event Action<bool> IsEnabledChanged;

  public virtual bool CanBeDisabled
  {
    get
    {
      return true;
    }
  }

  public bool IsActuallyEnabled
  {
    get
    {
      if (this._isActuallyEnabled != null)
      {
        return this._isActuallyEnabled.Value;
      }
      this._isActuallyEnabled = this.IsEnabled;
      return this._isActuallyEnabled.Value;
    }
    protected set
    {
      if (this._isActuallyEnabled == null)
      {
        this._isActuallyEnabled = value;
      }
    }
  }

  public bool IsEnabled
  {
    get
    {
      return this.ServiceSettingsBox.GetBool("IsEnabled", true);
    }
    set
    {
      this.IsActuallyEnabled = this.IsEnabled;
      this.ServiceSettingsBox.SetBool("IsEnabled", value);
      this.OnIsEnabledChanged(value);
    }
  }

  public string LuggageName { get; set; }

  public string ServiceDescription { get; protected set; }

  public string ServiceName { get; protected set; }

  protected virtual ISettingsBox ServiceSettingsBox
  {
    get
    {
      if (this._serviceSettingsBox != null)
      {
        return this._serviceSettingsBox;
      }
      var key = this.GetType().Name;
      var settingsRootBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox(AllServiceSettingsContainerName);
      this._serviceSettingsBox = settingsRootBox.GetSubBox(key);
      return this._serviceSettingsBox;
    }
  }

  public virtual TPlantLuggageType GetPlantLuggage(IPlantEx plantEx)
  {
    if (!plantEx.HasLuggage(this.LuggageName))
    {
      return null;
    }
    return plantEx.GetLuggage<TPlantLuggageType>(this.LuggageName);
  }

  public virtual void InformClosingStage()
  {
  }

  public virtual void InformDisplayStage()
  {
  }

  public virtual void InformInitializeStage()
  {
  }

  public virtual void InitializePlant(IPlantEx plantEx)
  {
    plantEx.EnabledChanged += this.PlantOnEnabledChanged;
  }

  public virtual bool IsAvailableForPlant(IPlantEx plantEx)
  {
    return this.GetPlantLuggage(plantEx) != null;
  }

  protected virtual void OnIsEnabledChanged(bool obj)
  {
    Action<bool> handler = this.IsEnabledChanged;
    if (handler != null)
    {
      handler(obj);
    }
  }

  protected virtual void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
  {
  }
}