using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TrayGarden.Reception.Services;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  public class ClipboardObserverPlantBox : ServicePlantBoxBase
  {
    public IClipboardListener WorksHungry { get; set; }

    public virtual void InformNewClipboardValue(string newClipboardValue)
    {
      if (this.IsEnabled)
      {
        Task.Factory.StartNew(() => this.WorksHungry.OnClipboardTextChanged(newClipboardValue));
      }
    }
  }
}