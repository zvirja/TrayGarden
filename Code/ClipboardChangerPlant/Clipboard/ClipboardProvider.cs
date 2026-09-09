using System;
using ClipboardChangerPlant.UIConfiguration;

using JetBrains.Annotations;

using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace ClipboardChangerPlant.Clipboard;

[UsedImplicitly]
public class ClipboardProvider : IClipboardWorks, IClipboardListener
{
  private static readonly string ListenClipboardSettingName = "Listen the clipboard";

  /// <summary>
  /// This event is raised even if update is initiated internally and actually hasn't been updated yet.
  /// Was introduced to properly handle context menu items relevance.
  /// </summary>
  public event Action<string> ClipboardValueUpdatedService;

  public event Action<string> OnClipboardValueChanged;

  public IClipboardProvider ActualProvider { get; set; }

  private bool ListenClipboad
  {
    get
    {
      return ListenClipoardSetting == null || ListenClipoardSetting.Value;
    }
  }

  private IBoolUserSetting ListenClipoardSetting { get; set; }

  public string GetValue()
  {
    return ActualProvider.GetCurrentClipboardText();
  }

  public void OnClipboardTextChanged(string newClipboardValue)
  {
    if (ListenClipboad)
    {
      OnClipboardValueUpdatedService(newClipboardValue);
      OnOnClipboardValueChanged(newClipboardValue);
    }
  }

  public void OnClipboardValueUpdatedService(string newValue)
  {
    Action<string> handler = ClipboardValueUpdatedService;
    if (handler != null)
    {
      handler(newValue);
    }
  }

  public void PostInit()
  {
    ListenClipoardSetting = UIConfigurationManager.ActualManager.SettingsSteward.DeclareBoolSetting("listenTheClipboard", ListenClipboardSettingName, true);
  }

  public void SetValue(string value, bool silent)
  {
    if (silent)
    {
      OnClipboardValueUpdatedService(value);
    }
    ActualProvider.SetCurrentClipboardText(value, silent);
  }

  public void StoreClipboardValueProvider(IClipboardProvider provider)
  {
    ActualProvider = provider;
  }

  private void OnOnClipboardValueChanged(string newValue)
  {
    Action<string> handler = OnClipboardValueChanged;
    if (handler != null)
    {
      handler(newValue);
    }
  }
}