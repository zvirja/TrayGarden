namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core;

public interface IClipboardProvider
{
  string GetCurrentClipboardText();

  string GetCurrentClipboardTextIgnoreSizeRestrictions();

  void SetCurrentClipboardText(string newValue, bool silent);
}