namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core;

public class ClipboardProvider : IClipboardProvider
{
  public ClipboardProvider(ClipboardObserverService service)
  {
    this.Service = service;
  }

  protected ClipboardObserverService Service { get; set; }

  public virtual string GetCurrentClipboardText()
  {
    return this.Service.GetClipboardValue(false);
  }

  public virtual string GetCurrentClipboardTextIgnoreSizeRestrictions()
  {
    return this.Service.GetClipboardValue(true);
  }

  public virtual void SetCurrentClipboardText(string newValue, bool silent)
  {
    this.Service.SetClipboardValue(newValue, silent);
  }
}