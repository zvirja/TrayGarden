using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core;

public class StandaloneIconPlantBox : ServicePlantBoxBase
{
  public StandaloneIconPlantBox()
  {
    base.IsEnabledChanged += this.StandaloneIconPlantBox_IsEnabledChanged;
  }

  public NotifyIcon NotifyIcon { get; set; }

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

  protected virtual void StandaloneIconPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newvalue)
  {
    this.FixNIVisibility();
  }
}