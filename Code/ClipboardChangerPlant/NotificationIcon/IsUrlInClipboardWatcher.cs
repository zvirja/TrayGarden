using System;
using ClipboardChangerPlant.Clipboard;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

namespace ClipboardChangerPlant.NotificationIcon;

public class IsUrlInClipboardWatcher : IDynamicStateProvider
{
  public IsUrlInClipboardWatcher()
  {
    ClipboardManager.Provider.ClipboardValueUpdatedService += ProviderOnClipboardValueUpdatedService;
    CurrentRelevanceLevel = RelevanceLevel.Normal;
  }

  public event EventHandler RelevanceChanged;

  public RelevanceLevel CurrentRelevanceLevel { get; private set; }

  private bool IsValidUrl(string newValue)
  {
    return newValue.StartsWith("http://") || newValue.StartsWith("https://");
  }

  private void OnRelevanceChanged()
  {
    EventHandler handler = RelevanceChanged;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  private void ProviderOnClipboardValueUpdatedService(string newValue)
  {
    if (IsValidUrl(newValue))
    {
      CurrentRelevanceLevel = RelevanceLevel.Normal;
    }
    else
    {
      CurrentRelevanceLevel = RelevanceLevel.Low;
    }
    OnRelevanceChanged();
  }
}