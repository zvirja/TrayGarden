using System;
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
    ServiceName = serviceName;
    LuggageName = luggageName;
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
      if (_isActuallyEnabled != null)
      {
        return _isActuallyEnabled.Value;
      }
      _isActuallyEnabled = IsEnabled;
      return _isActuallyEnabled.Value;
    }
    protected set
    {
      if (_isActuallyEnabled == null)
      {
        _isActuallyEnabled = value;
      }
    }
  }

  public bool IsEnabled
  {
    get
    {
      return ServiceSettingsBox.GetBool("IsEnabled", true);
    }
    set
    {
      IsActuallyEnabled = IsEnabled;
      ServiceSettingsBox.SetBool("IsEnabled", value);
      OnIsEnabledChanged(value);
    }
  }

  public string LuggageName { get; set; }

  public string ServiceDescription { get; protected set; }

  public string ServiceName { get; protected set; }

  protected virtual ISettingsBox ServiceSettingsBox
  {
    get
    {
      if (_serviceSettingsBox != null)
      {
        return _serviceSettingsBox;
      }
      var key = GetType().Name;
      var settingsRootBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox(AllServiceSettingsContainerName);
      _serviceSettingsBox = settingsRootBox.GetSubBox(key);
      return _serviceSettingsBox;
    }
  }

  public virtual TPlantLuggageType GetPlantLuggage(IPlantEx plantEx)
  {
    if (!plantEx.HasLuggage(LuggageName))
    {
      return null;
    }
    return plantEx.GetLuggage<TPlantLuggageType>(LuggageName);
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
    plantEx.EnabledChanged += PlantOnEnabledChanged;
  }

  public virtual bool IsAvailableForPlant(IPlantEx plantEx)
  {
    return GetPlantLuggage(plantEx) != null;
  }

  protected virtual void OnIsEnabledChanged(bool obj)
  {
    Action<bool> handler = IsEnabledChanged;
    if (handler != null)
    {
      handler(obj);
    }
  }

  protected virtual void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
  {
  }
}