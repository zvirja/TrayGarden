using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using TrayGarden.RuntimeSettings.Provider;

namespace TrayGarden.RuntimeSettings;

public class ContainerBasedSettingsBox : ISettingsBox
{
  public ContainerBasedSettingsBox()
  {
    this.SubBoxes = new Dictionary<string, ContainerBasedSettingsBox>();
  }

  public event Action OnSaving;

  protected ISettingsBox ParentBox { get; set; }

  protected Dictionary<string, ContainerBasedSettingsBox> SubBoxes { get; set; }

  protected IContainer UnderlyingContainer { get; set; }

  public virtual string this[string settingName]
  {
    get
    {
      return this.UnderlyingContainer.GetStringSetting(settingName);
    }
    set
    {
      this.UnderlyingContainer.SetStringSetting(settingName, value);
      if (BulkSettingsUpdate.CurrentValue != BulkUpdateState.Enabled)
      {
        this.Save();
      }
    }
  }

  public virtual bool GetBool(string settingName, bool fallbackValue)
  {
    bool value;
    return this.TryGetBool(settingName, out value) ? value : fallbackValue;
  }

  public double GetDouble(string settingName, double fallbackValue)
  {
    double value;
    return this.TryGetDouble(settingName, out value) ? value : fallbackValue;
  }

  public virtual int GetInt(string settingName, int fallbackValue)
  {
    int value;
    return this.TryGetInt(settingName, out value) ? value : fallbackValue;
  }

  public virtual string GetString(string settingName, string fallbackValue)
  {
    var value = this[settingName];
    return value ?? fallbackValue;
  }

  public virtual ISettingsBox GetSubBox(string boxName)
  {
    var boxNameUppercased = boxName.ToLowerInvariant();
    if (this.SubBoxes.ContainsKey(boxNameUppercased))
    {
      return this.SubBoxes[boxName];
    }
    var subContainer = this.UnderlyingContainer.GetNamedSubContainer(boxNameUppercased);
    var newBox = new ContainerBasedSettingsBox();
    newBox.Initialize(subContainer);
    newBox.ParentBox = this;
    this.SubBoxes[boxName] = newBox;
    return newBox;
  }

  public virtual void Initialize(IContainer container)
  {
    this.UnderlyingContainer = container;
  }

  public virtual void Save()
  {
    this.CallOnSaving();
    if (this.ParentBox != null)
    {
      this.ParentBox.Save();
    }
  }

  public virtual void SetBool(string settingName, bool value)
  {
    this[settingName] = value.ToString(CultureInfo.InvariantCulture);
  }

  public void SetDouble(string settingName, double value)
  {
    this[settingName] = value.ToString(CultureInfo.InvariantCulture);
  }

  public virtual void SetInt(string settingName, int value)
  {
    this[settingName] = value.ToString(CultureInfo.InvariantCulture);
  }

  public virtual void SetString(string settingName, string settingValue)
  {
    this[settingName] = settingValue;
  }

  public virtual bool TryGetBool(string settingName, out bool value)
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

  public virtual bool TryGetInt(string settingName, out int value)
  {
    if (int.TryParse(this[settingName], out value))
    {
      return true;
    }
    return false;
  }

  protected virtual void CallOnSaving()
  {
    Action handler = this.OnSaving;
    if (handler != null)
    {
      handler();
    }
  }
}