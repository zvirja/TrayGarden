using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationPlayer : IConfigurationPlayerWithValues
  {
    #region Fields

    protected bool _requiresApplicationReboot;

    #endregion

    #region Constructors and Destructors

    public ConfigurationPlayer([NotNull] string settingName, bool supportsReset, bool readOnly)
    {
      Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
      this.SettingName = settingName;
      this.SupportsReset = supportsReset;
      this.ReadOnly = readOnly;
      this.AdditionalActions = new List<IConfigurationEntryAction>();
    }

    #endregion

    #region Public Events

    public event Action RequiresApplicationRebootChanged;

    public event Action ValueChanged;

    #endregion

    #region Public Properties

    public ICommand Action { get; protected set; }

    public string ActionTitle { get; protected set; }

    public List<IConfigurationEntryAction> AdditionalActions { get; set; }

    public virtual bool BoolValue { get; set; }

    public virtual double DoubleValue { get; set; }

    public bool HideReset { get; protected set; }

    public virtual int IntValue { get; set; }

    public virtual object ObjectValue { get; set; }

    public bool ReadOnly { get; set; }

    public virtual bool RequiresApplicationReboot
    {
      get
      {
        return this._requiresApplicationReboot;
      }
      set
      {
        this._requiresApplicationReboot = value;
        this.OnRequiresApplicationRebootChanged();
      }
    }

    public virtual string SettingDescription { get; protected set; }

    public string SettingName { get; protected set; }

    public virtual string StringOptionValue { get; set; }

    public virtual object StringOptions { get; protected set; }

    public virtual string StringValue { get; set; }

    public bool SupportsReset { get; protected set; }

    #endregion

    #region Public Methods and Operators

    public virtual void Reset()
    {
    }

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