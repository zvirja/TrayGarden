#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TrayGarden.Services.FleaMarket.IconChanger;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core
{
  public class GlobalMenuPlantBox : ServicePlantBoxBase
  {
    #region Fields

    private INotifyIconChangerMaster _globalNotifyIconChanger;

    #endregion

    #region Constructors and Destructors

    public GlobalMenuPlantBox()
    {
      base.IsEnabledChanged += this.GlobalMenuPlantBox_IsEnabledChanged;
    }

    #endregion

    #region Public Properties

    public INotifyIconChangerMaster GlobalNotifyIconChanger
    {
      get
      {
        return this._globalNotifyIconChanger;
      }
      set
      {
        this._globalNotifyIconChanger = value;
        if (this._globalNotifyIconChanger != null)
        {
          this._globalNotifyIconChanger.IsEnabled = this.GlobalNotifyIconChangerEnabled;
        }
      }
    }

    public bool GlobalNotifyIconChangerEnabled
    {
      get
      {
        bool isEnabled = this.SettingsBox.GetBool("notifyIconChangerEnabled", true);
        if (this.GlobalNotifyIconChanger != null)
        {
          this.GlobalNotifyIconChanger.IsEnabled = isEnabled;
        }
        return isEnabled;
      }
      set
      {
        this.SettingsBox.SetBool("notifyIconChangerEnabled", value);
        if (this.GlobalNotifyIconChanger != null)
        {
          this.GlobalNotifyIconChanger.IsEnabled = value;
        }
      }
    }

    public bool IsGlobalIconChangingEnabled { get; set; }

    public List<ToolStripItem> ToolStripMenuItems { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual void FixVisibility()
    {
      if (this.ToolStripMenuItems == null)
      {
        return;
      }
      var shouldBeVisible = this.RelatedPlantEx.IsEnabled && this.IsEnabled;
      foreach (ToolStripItem toolStripMenuItem in this.ToolStripMenuItems)
      {
        toolStripMenuItem.Visible = shouldBeVisible;
      }
    }

    #endregion

    #region Methods

    private void GlobalMenuPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newValue)
    {
      this.FixVisibility();
    }

    #endregion
  }
}