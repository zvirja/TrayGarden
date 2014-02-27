#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TrayGarden.Reception.Services;

#endregion

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  public class ClipboardObserverPlantBox : ServicePlantBoxBase
  {
    #region Public Properties

    public IClipboardListener WorksHungry { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual void InformNewClipboardValue(string newClipboardValue)
    {
      if (this.IsEnabled)
      {
        Task.Factory.StartNew(() => this.WorksHungry.OnClipboardTextChanged(newClipboardValue));
      }
    }

    #endregion
  }
}