using System;
using System.Collections.Generic;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;

namespace TrayGarden.UI.Configuration.EntryVM.Players;

public abstract class ConfigurationPlayerBase : IConfigurationPlayer
{
  private List<IConfigurationEntryAction> additionalActions;

  private bool supportsReset;

  public ConfigurationPlayerBase([NotNull] string settingName, bool supportsReset, bool readOnly)
  {
    Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
    SettingName = settingName;
    this.supportsReset = supportsReset;
    ReadOnly = readOnly;
    additionalActions = new List<IConfigurationEntryAction>();
  }

  public event Action RequiresApplicationRebootChanged;

  public event Action ValueChanged;

  public virtual List<IConfigurationEntryAction> AdditionalActions
  {
    get
    {
      return additionalActions;
    }
    protected set
    {
      additionalActions = value;
    }
  }

  public virtual bool HideReset { get; protected set; }

  public bool ReadOnly { get; protected set; }

  public virtual bool RequiresApplicationReboot { get; protected set; }

  public virtual string SettingDescription { get; protected set; }

  public string SettingName { get; protected set; }

  public virtual bool SupportsReset
  {
    get
    {
      return supportsReset;
    }
    protected set
    {
      supportsReset = value;
    }
  }

  public abstract void Reset();

  protected virtual void OnRequiresApplicationRebootChanged()
  {
    Action handler = RequiresApplicationRebootChanged;
    if (handler != null)
    {
      handler();
    }
  }

  protected virtual void OnValueChanged()
  {
    Action handler = ValueChanged;
    if (handler != null)
    {
      handler();
    }
  }
}