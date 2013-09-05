using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.UIConfiguration;
using JetBrains.Annotations;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace ClipboardChangerPlant.Clipboard
{
  [UsedImplicitly]
  public class ClipboardProvider : IClipboardWorks, IClipboardListener
  {
    protected static readonly string ListenClipboardSettingName = "Listen the clipboard";
    protected IUserSetting ListenClipoardSetting { get; set; }

    protected bool ListenClipboad
    {
      get
      {
        return ListenClipoardSetting == null || ListenClipoardSetting.BoolValue;
      }
    }

    public IClipboardProvider ActualProvider { get; set; }
    public event Action<string> OnClipboardValueChanged;

    public virtual string GetValue()
    {
      return ActualProvider.GetCurrentClipboardText();
    }

    public virtual void SetValue(string value, bool silent)
    {
      ActualProvider.SetCurrentClipboardText(value, silent);
    }

    public virtual void PreInit()
    {
      UIConfigurationManager.ActualManager.VolatileUserSettings.Add(EmbedUserSettings);
    }

    public virtual void PostInit()
    {
      ListenClipoardSetting = UIConfigurationManager.ActualManager.UserSettings[ListenClipboardSettingName];
    }

    public virtual void OnClipboardTextChanged(string newClipboardValue)
    {
      if(ListenClipboad)
        OnOnClipboardValueChanged(newClipboardValue);
    }

    public virtual void StoreClipboardValueProvider(IClipboardProvider provider)
    {
      ActualProvider = provider;
    }

    protected virtual void EmbedUserSettings(IUserSettingsMetadataBuilder userSettingsMetadataBuilder)
    {
      userSettingsMetadataBuilder.DeclareBoolSetting(ListenClipboardSettingName, true);
    }

    protected virtual void OnOnClipboardValueChanged(string newValue)
    {
      Action<string> handler = OnClipboardValueChanged;
      if (handler != null) handler(newValue);
    }
  }
}
