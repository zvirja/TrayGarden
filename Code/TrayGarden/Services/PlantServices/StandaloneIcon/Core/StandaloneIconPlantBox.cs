using System.Windows.Forms;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core;

public class StandaloneIconPlantBox : ServicePlantBoxBase
{
  public StandaloneIconPlantBox()
  {
    IsEnabledChanged += StandaloneIconPlantBox_IsEnabledChanged;
  }

  public NotifyIcon NotifyIcon { get; set; }

  public virtual void FixNIVisibility()
  {
    if (RelatedPlantEx.IsEnabled)
    {
      NotifyIcon.Visible = IsEnabled;
    }
    else
    {
      NotifyIcon.Visible = false;
    }
  }

  protected virtual void StandaloneIconPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newvalue)
  {
    FixNIVisibility();
  }
}