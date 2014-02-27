#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#endregion

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core
{
  public class StandaloneIconPlantBox : ServicePlantBoxBase
  {
    #region Constructors and Destructors

    public StandaloneIconPlantBox()
    {
      base.IsEnabledChanged += this.StandaloneIconPlantBox_IsEnabledChanged;
    }

    #endregion

    #region Public Properties

    public NotifyIcon NotifyIcon { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual void FixNIVisibility()
    {
      if (this.RelatedPlantEx.IsEnabled)
      {
        this.NotifyIcon.Visible = this.IsEnabled;
      }
      else
      {
        this.NotifyIcon.Visible = false;
      }
    }

    #endregion

    #region Methods

    protected virtual void StandaloneIconPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newvalue)
    {
      this.FixNIVisibility();
    }

    #endregion
  }
}