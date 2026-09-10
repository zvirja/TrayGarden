using System;
using System.Collections.Generic;
using System.Globalization;
using TrayGarden.RuntimeSettings.Provider;

namespace TrayGarden.RuntimeSettings;

public class ContainerBasedSettingsBox : ISettingsBox
{
  public ContainerBasedSettingsBox()
  {
    SubBoxes = new Dictionary<string, ContainerBasedSettingsBox>();
  }

  public event Action OnSaving;

  private ISettingsBox ParentBox { get; set; }

  private Dictionary<string, ContainerBasedSettingsBox> SubBoxes { get; set; }

  private IContainer UnderlyingContainer { get; set; }

  public string this[string settingName]
  {
    get
    {
      return UnderlyingContainer.GetStringSetting(settingName);
    }
    set
    {
      UnderlyingContainer.SetStringSetting(settingName, value);
      if (BulkSettingsUpdate.CurrentValue != BulkUpdateState.Enabled)
      {
        Save();
      }
    }
  }

  public bool GetBool(string settingName, bool fallbackValue)
  {
    bool value;
    return TryGetBool(settingName, out value) ? value : fallbackValue;
  }

  public double GetDouble(string settingName, double fallbackValue)
  {
    double value;
    return TryGetDouble(settingName, out value) ? value : fallbackValue;
  }

  public int GetInt(string settingName, int fallbackValue)
  {
    int value;
    return TryGetInt(settingName, out value) ? value : fallbackValue;
  }

  public string GetString(string settingName, string fallbackValue)
  {
    var value = this[settingName];
    return value ?? fallbackValue;
  }

  public ISettingsBox GetSubBox(string boxName)
  {
    var boxNameUppercased = boxName.ToLowerInvariant();
    if (SubBoxes.ContainsKey(boxNameUppercased))
    {
      return SubBoxes[boxName];
    }
    var subContainer = UnderlyingContainer.GetNamedSubContainer(boxNameUppercased);
    var newBox = new ContainerBasedSettingsBox();
    newBox.Initialize(subContainer);
    newBox.ParentBox = this;
    SubBoxes[boxName] = newBox;
    return newBox;
  }

  public void Initialize(IContainer container)
  {
    UnderlyingContainer = container;
  }

  public void Save()
  {
    CallOnSaving();
    if (ParentBox != null)
    {
      ParentBox.Save();
    }
  }

  public void SetBool(string settingName, bool value)
  {
    this[settingName] = value.ToString(CultureInfo.InvariantCulture);
  }

  public void SetDouble(string settingName, double value)
  {
    this[settingName] = value.ToString(CultureInfo.InvariantCulture);
  }

  public void SetInt(string settingName, int value)
  {
    this[settingName] = value.ToString(CultureInfo.InvariantCulture);
  }

  public void SetString(string settingName, string settingValue)
  {
    this[settingName] = settingValue;
  }

  public bool TryGetBool(string settingName, out bool value)
  {
    if (bool.TryParse(this[settingName], out value))
    {
      return true;
    }
    return false;
  }

  public bool TryGetDouble(string settingName, out double value)
  {
    if (double.TryParse(this[settingName], out value))
    {
      return true;
    }
    return false;
  }

  public bool TryGetInt(string settingName, out int value)
  {
    if (int.TryParse(this[settingName], out value))
    {
      return true;
    }
    return false;
  }

  private void CallOnSaving()
  {
    Action handler = OnSaving;
    if (handler != null)
    {
      handler();
    }
  }
}