using System.Windows.Forms;

using TrayGarden.Reception.Services.StandaloneIcon;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core;

public class StandaloneIconPlantBox : ServicePlantBoxBase
{
  public StandaloneIconPlantBox()
  {
    IsEnabledChanged += StandaloneIconPlantBox_IsEnabledChanged;
  }

  public NotifyIcon NotifyIcon { get; set; }

  public void FixNIVisibility()
  {
    if (RelatedPlantEx.IsEnabled)
    {
      var visibilityControl = RelatedPlantEx.GetFirstWorkhorseOfType<INotifyIconVisibilityControl>();
      NotifyIcon.Visible = IsEnabled && (visibilityControl == null || visibilityControl.IsIconVisible);
    }
    else
    {
      NotifyIcon.Visible = false;
    }
  }

  private void StandaloneIconPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newvalue)
  {
    FixNIVisibility();
  }
}