using System.Collections.Generic;
using TrayGarden.Reception;

namespace TelegramToggleWindowHook;

public class Plant : IPlant, IServicesDelegation
{
  private KeyboardHookProcessingForm _form;

  public string Description => "Plugin hooks the Ctrl+` shortcut and toggles the configured app window state.\nIs active even if you see that it's disabled here.";

  public string HumanSupportingName => "Toggle main window by shortcut";

  public void Initialize()
  {
    this._form = new KeyboardHookProcessingForm();
  }

  public void PostServicesInitialize()
  {
  }

  public List<object> GetServiceDelegates()
  {
    return [PlantConfiguration.Instance];
  }
}