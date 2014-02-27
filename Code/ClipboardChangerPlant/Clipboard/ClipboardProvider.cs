#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.UIConfiguration;

using JetBrains.Annotations;

using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

#endregion

namespace ClipboardChangerPlant.Clipboard
{
  [UsedImplicitly]
  public class ClipboardProvider : IClipboardWorks, IClipboardListener
  {
    #region Static Fields

    protected static readonly string ListenClipboardSettingName = "Listen the clipboard";

    #endregion

    #region Public Events

    public event Action<string> OnClipboardValueChanged;

    #endregion

    #region Public Properties

    public IClipboardProvider ActualProvider { get; set; }

    #endregion

    #region Properties

    protected bool ListenClipboad
    {
      get
      {
        return this.ListenClipoardSetting == null || this.ListenClipoardSetting.Value;
      }
    }

    protected IBoolUserSetting ListenClipoardSetting { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual string GetValue()
    {
      return this.ActualProvider.GetCurrentClipboardText();
    }

    public virtual void OnClipboardTextChanged(string newClipboardValue)
    {
      if (this.ListenClipboad)
      {
        this.OnOnClipboardValueChanged(newClipboardValue);
      }
    }

    public virtual void PostInit()
    {
      this.ListenClipoardSetting = UIConfigurationManager.ActualManager.SettingsSteward.DeclareBoolSetting(
        "listenTheClipboard",
        ListenClipboardSettingName,
        true);
    }

    public virtual void SetValue(string value, bool silent)
    {
      this.ActualProvider.SetCurrentClipboardText(value, silent);
    }

    public virtual void StoreClipboardValueProvider(IClipboardProvider provider)
    {
      this.ActualProvider = provider;
    }

    #endregion

    #region Methods

    protected virtual void OnOnClipboardValueChanged(string newValue)
    {
      Action<string> handler = this.OnClipboardValueChanged;
      if (handler != null)
      {
        handler(newValue);
      }
    }

    #endregion
  }
}