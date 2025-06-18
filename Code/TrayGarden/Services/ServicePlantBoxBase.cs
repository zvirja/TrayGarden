using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services;

public class ServicePlantBoxBase
{
  public delegate void ServicePlantBoxEnabledChanged(ServicePlantBoxBase sender, bool newValue);

  public event ServicePlantBoxEnabledChanged IsEnabledChanged;

  public virtual bool IsEnabled
  {
    get
    {
      return SettingsBox.GetBool("enabled", true);
    }
    //TODO FIX BUG. Settings Box may be null if we just set initial IsEnabled value
    set
    {
      SettingsBox.SetBool("enabled", value);
      OnIsEnabledChanged(value);
    }
  }

  public IPlantEx RelatedPlantEx { get; set; }

  public ISettingsBox SettingsBox { get; set; }

  protected virtual void OnIsEnabledChanged(bool newValue)
  {
    ServicePlantBoxEnabledChanged handler = IsEnabledChanged;
    if (handler != null)
    {
      handler(this, newValue);
    }
  }
}