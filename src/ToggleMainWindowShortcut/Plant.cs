using System.Collections.Generic;
using TrayGarden.Reception;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.IsEnabledObserver;

namespace TelegramToggleWindowHook;

public class Plant : IPlant, IServicesDelegation, IIsEnabledObserver
{
  private KeyboardHookProcessingForm _form;

  public string Description => "Plugin hooks the Ctrl+` shortcut and toggles the configured app window state.";

  public string HumanSupportingName => "Toggle main window by shortcut";
  
  public IPlantEnabledInfo IsEnabledInfo { get; set; }

  public void Initialize()
  {
  }

  public void PostServicesInitialize()
  {
    if (IsEnabledInfo.IsEnabled)
    {
      _form = new KeyboardHookProcessingForm();
    }

    IsEnabledInfo.IsEnabledChanged += (sender, args) =>
    {
      if (IsEnabledInfo.IsEnabled)
      {
        _form ??= new KeyboardHookProcessingForm();
      }
      else
      {
        _form?.Dispose();
        _form = null;
      }
    };
  }

  public List<object> GetServiceDelegates()
  {
    return [PlantConfiguration.Instance];
  }

  public void ConsumeIsEnabledInfo(IPlantEnabledInfo plantEnabledInfo)
  {
    IsEnabledInfo = plantEnabledInfo;
  }
}