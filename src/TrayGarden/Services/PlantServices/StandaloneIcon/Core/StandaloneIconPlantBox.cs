using System.Windows.Forms;

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
      NotifyIcon.Visible = IsEnabled;
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