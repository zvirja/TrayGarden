#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;

#endregion

namespace TrayGarden.Services
{
  public class ServicePlantBoxBase
  {
    #region Delegates

    public delegate void ServicePlantBoxEnabledChanged(ServicePlantBoxBase sender, bool newValue);

    #endregion

    #region Public Events

    public event ServicePlantBoxEnabledChanged IsEnabledChanged;

    #endregion

    #region Public Properties

    public virtual bool IsEnabled
    {
      get
      {
        return this.SettingsBox.GetBool("enabled", true);
      }
      //TODO FIX BUG. Settings Box may be null if we just set initial IsEnabled value
      set
      {
        this.SettingsBox.SetBool("enabled", value);
        this.OnIsEnabledChanged(value);
      }
    }

    public IPlantEx RelatedPlantEx { get; set; }

    public ISettingsBox SettingsBox { get; set; }

    #endregion

    #region Methods

    protected virtual void OnIsEnabledChanged(bool newValue)
    {
      ServicePlantBoxEnabledChanged handler = this.IsEnabledChanged;
      if (handler != null)
      {
        handler(this, newValue);
      }
    }

    #endregion
  }
}