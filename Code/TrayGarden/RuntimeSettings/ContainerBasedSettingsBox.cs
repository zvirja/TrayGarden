using System;
using System.Collections.Generic;
using System.Globalization;
using TrayGarden.RuntimeSettings.Provider;

namespace TrayGarden.RuntimeSettings
{
  public class ContainerBasedSettingsBox : ISettingsBox
  {
    protected IContainer UnderlyingContainer { get; set; }
    protected Dictionary<string, ContainerBasedSettingsBox> SubBoxes { get; set; }
    protected ISettingsBox ParentBox { get; set; }

    public event Action OnSaving;

    public ContainerBasedSettingsBox()
    {
      SubBoxes = new Dictionary<string, ContainerBasedSettingsBox>();
    }

    public virtual void Initialize(IContainer container)
    {
      UnderlyingContainer = container;
    }

    public virtual string this[string settingName]
    {
      get { return UnderlyingContainer.GetStringSetting(settingName); }
      set
      {
        UnderlyingContainer.SetStringSetting(settingName, value);
        if (BulkSettingsUpdate.CurrentValue != BulkUpdateState.Enabled)
          Save();
      }
    }

    public virtual string GetString(string settingName, string fallbackValue)
    {
      var value = this[settingName];
      return value ?? fallbackValue;
    }

    public virtual void SetString(string settingName, string settingValue)
    {
      this[settingName] = settingValue;
    }

    public virtual int GetInt(string settingName, int fallbackValue)
    {
      int value;
      return TryGetInt(settingName, out value) ? value : fallbackValue;
    }

    public virtual void SetInt(string settingName, int value)
    {
      this[settingName] = value.ToString(CultureInfo.InvariantCulture);
    }

    public double GetDouble(string settingName, double fallbackValue)
    {
      double value;
      return TryGetDouble(settingName, out value) ? value : fallbackValue;
    }

    public void SetDouble(string settingName, double value)
    {
      this[settingName] = value.ToString(CultureInfo.InvariantCulture);
    }

    public virtual bool GetBool(string settingName, bool fallbackValue)
    {
      bool value;
      return TryGetBool(settingName, out value) ? value : fallbackValue;
    }

    public virtual void SetBool(string settingName, bool value)
    {
      this[settingName] = value.ToString(CultureInfo.InvariantCulture);
    }

    public virtual bool TryGetBool(string settingName, out bool value)
    {
      if (bool.TryParse(this[settingName], out value))
        return true;
      return false;
    }

    public virtual bool TryGetInt(string settingName, out int value)
    {
      if (int.TryParse(this[settingName], out value))
        return true;
      return false;
    }

    public bool TryGetDouble(string settingName, out double value)
    {
      if (double.TryParse(this[settingName], out value))
        return true;
      return false;
    }

    public virtual ISettingsBox GetSubBox(string boxName)
    {
      var boxNameUppercased = boxName.ToLowerInvariant();
      if (SubBoxes.ContainsKey(boxNameUppercased))
        return SubBoxes[boxName];
      var subContainer = UnderlyingContainer.GetNamedSubContainer(boxNameUppercased);
      var newBox = new ContainerBasedSettingsBox();
      newBox.Initialize(subContainer);
      newBox.ParentBox = this;
      SubBoxes[boxName] = newBox;
      return newBox;
    }

    public virtual void Save()
    {
      CallOnSaving();
      if (ParentBox != null)
        ParentBox.Save();
    }

    protected virtual void CallOnSaving()
    {
      Action handler = OnSaving;
      if (handler != null) handler();
    }
  }
}
