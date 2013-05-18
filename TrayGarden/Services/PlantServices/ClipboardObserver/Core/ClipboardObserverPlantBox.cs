using System.Threading.Tasks;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.ClipboardObserver.Smorgasbord;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
    public class ClipboardObserverPlantBox
    {
        public IPlantEx RelatedPlant { get; set; }
        public IAskClipboardEvents EventsHungry { get; set; }
        
        public virtual void InformNewClipboardValue(string newClipboardValue)
        {
            Task.Factory.StartNew(() => EventsHungry.OnClipboardTextChanged(newClipboardValue));
        }
    }
}
