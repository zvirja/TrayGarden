using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public abstract class ConfigurationPlayerBase : IConfigurationPlayer
  {
    private List<IConfigurationEntryAction> additionalActions;

    private bool supportsReset;

    public ConfigurationPlayerBase([NotNull] string settingName, bool supportsReset, bool readOnly)
    {
      Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
      this.SettingName = settingName;
      this.supportsReset = supportsReset;
      this.ReadOnly = readOnly;
      this.additionalActions = new List<IConfigurationEntryAction>();
    }

    public event Action RequiresApplicationRebootChanged;

    public event Action ValueChanged;

    public virtual List<IConfigurationEntryAction> AdditionalActions
    {
      get
      {
        return this.additionalActions;
      }
      protected set
      {
        this.additionalActions = value;
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
        return this.supportsReset;
      }
      protected set
      {
        this.supportsReset = value;
      }
    }

    public abstract void Reset();

    protected virtual void OnRequiresApplicationRebootChanged()
    {
      Action handler = this.RequiresApplicationRebootChanged;
      if (handler != null)
      {
        handler();
      }
    }

    protected virtual void OnValueChanged()
    {
      Action handler = this.ValueChanged;
      if (handler != null)
      {
        handler();
      }
    }
  }
}