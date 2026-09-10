namespace TrayGarden.Reception.Services;

/// <summary>
/// This service allows to listen the clipboard events. 
/// If service is enabled you will receive a notification each time the Clipboard content is changed.
/// </summary>
public interface IClipboardListener
{
  void OnClipboardTextChanged(string newClipboardValue);
}