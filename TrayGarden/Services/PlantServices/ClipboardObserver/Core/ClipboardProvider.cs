namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  public class ClipboardProvider : IClipboardProvider
  {
    protected ClipboardObserverService Service { get; set; }
    public ClipboardProvider(ClipboardObserverService service)
    {
      Service = service;
    }

    public virtual string GetCurrentClipboardText()
    {
      return Service.GetLastTimeClipboardValue();
    }

    public virtual void SetCurrentClipboardText(string newValue, bool silent)
    {
      Service.QueueNewClipboardValue(newValue,silent);
    }
  }
}