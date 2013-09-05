using System.Threading.Tasks;
using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  public class ClipboardObserverPlantBox : ServicePlantBoxBase
  {
    public IClipboardListener WorksHungry { get; set; }

    public virtual void InformNewClipboardValue(string newClipboardValue)
    {
      if (IsEnabled)
        Task.Factory.StartNew(() => WorksHungry.OnClipboardTextChanged(newClipboardValue));
    }
  }
}
