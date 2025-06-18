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

  public RelevanceLevel CurrentRelevanceLevel { get; protected set; }

  protected virtual bool IsValidUrl(string newValue)
  {
    return newValue.StartsWith("http://") || newValue.StartsWith("https://");
  }

  protected virtual void OnRelevanceChanged()
  {
    EventHandler handler = RelevanceChanged;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  protected virtual void ProviderOnClipboardValueUpdatedService(string newValue)
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