namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core;

public class ClipboardProvider : IClipboardProvider
{
  public ClipboardProvider(ClipboardObserverService service)
  {
    Service = service;
  }

  private ClipboardObserverService Service { get; set; }

  public string GetCurrentClipboardText()
  {
    return Service.GetClipboardValue(false);
  }

  public string GetCurrentClipboardTextIgnoreSizeRestrictions()
  {
    return Service.GetClipboardValue(true);
  }

  public void SetCurrentClipboardText(string newValue, bool silent)
  {
    Service.SetClipboardValue(newValue, silent);
  }
}