using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Clipboard;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

namespace ClipboardChangerPlant.NotificationIcon
{
  public class IsUrlInClipboardWatcher : IDynamicStateProvider
  {
    public IsUrlInClipboardWatcher()
    {
      ClipboardManager.Provider.ClipboardValueUpdatedService += this.ProviderOnClipboardValueUpdatedService;
      this.CurrentRelevanceLevel = RelevanceLevel.Normal;
    }

    public event EventHandler RelevanceChanged;

    public RelevanceLevel CurrentRelevanceLevel { get; protected set; }

    protected virtual bool IsValidUrl(string newValue)
    {
      return newValue.StartsWith("http://") || newValue.StartsWith("https://");
    }

    protected virtual void OnRelevanceChanged()
    {
      EventHandler handler = this.RelevanceChanged;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }

    protected virtual void ProviderOnClipboardValueUpdatedService(string newValue)
    {
      if (this.IsValidUrl(newValue))
      {
        this.CurrentRelevanceLevel = RelevanceLevel.Normal;
      }
      else
      {
        this.CurrentRelevanceLevel = RelevanceLevel.Low;
      }
      this.OnRelevanceChanged();
    }
  }
}