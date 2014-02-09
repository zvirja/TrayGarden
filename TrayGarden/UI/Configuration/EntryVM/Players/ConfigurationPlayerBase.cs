#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public abstract class ConfigurationPlayerBase : IConfigurationPlayer
  {
    #region Fields

    private List<IConfigurationEntryAction> additionalActions;

    private bool supportsReset;

    #endregion

    #region Constructors and Destructors

    public ConfigurationPlayerBase([NotNull] string settingName, bool supportsReset, bool readOnly)
    {
      Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
      this.SettingName = settingName;
      this.supportsReset = supportsReset;
      this.ReadOnly = readOnly;
      this.additionalActions = new List<IConfigurationEntryAction>();
    }

    #endregion

    #region Public Events

    public event Action RequiresApplicationRebootChanged;

    public event Action ValueChanged;

    #endregion

    #region Public Properties

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

    #endregion

    #region Public Methods and Operators

    public abstract void Reset();

    #endregion

    #region Methods

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

    #endregion
  }
}