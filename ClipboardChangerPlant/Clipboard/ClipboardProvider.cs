using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ClipboardChangerPlant.Configuration;
using JetBrains.Annotations;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;

namespace ClipboardChangerPlant.Clipboard
{
  [UsedImplicitly]
  public class ClipboardProvider:IClipboardWorks
  {
    public IClipboardProvider ActualProvider { get; set; }
    public event Action<string> OnClipboardValueChanged;

    public virtual string GetValue()
    {
      return ActualProvider.GetCurrentClipboardText();
    }

    public virtual void SetValue(string value)
    {
      ActualProvider.SetCurrentClipboardText(value);
    }


    public virtual void OnClipboardTextChanged(string newClipboardValue)
    {
      OnOnClipboardValueChanged(newClipboardValue);
    }

    public virtual void StoreClipboardValueProvider(IClipboardProvider provider)
    {
      ActualProvider = provider;
    }

    protected virtual void OnOnClipboardValueChanged(string newValue)
    {
      Action<string> handler = OnClipboardValueChanged;
      if (handler != null) handler(newValue);
    }
  }
}
